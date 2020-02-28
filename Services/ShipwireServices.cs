using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Nop.Core.Domain.Shipping;
using Nop.Services.Shipping;
using Nop.Services.Logging;

namespace Nop.Plugin.OrderFullfillment.Shipwire.Services
{
    class ShipwireServices
    {
        #region Fields

        private readonly HttpClient _httpClient;
        private readonly ShipwireSettings _shipwireSettings;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public ShipwireServices(HttpClient client)
        {
            //configure client
            client.Timeout = TimeSpan.FromMilliseconds(5000);
            //client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, $"nopCommerce-{NopVersion.CurrentVersion}");

            _httpClient = client;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Gets shipping rates
        /// </summary>
        /// <param name="shippingOptionRequest">Shipping option request details</param>
        /// <returns>Represents a response of getting shipping rate options</returns>
        /*
        public virtual GetShippingOptionResponse GetRates(GetShippingOptionRequest shippingOptionRequest)
        {
            var response = new GetShippingOptionResponse();

            //get regular rates
            var (shippingOptions, error) = GetShippingOptions(shippingOptionRequest);
            response.ShippingOptions = shippingOptions;
            if (!string.IsNullOrEmpty(error))
                response.Errors.Add(error);

            if (response.ShippingOptions.Any())
                response.Errors.Clear();

            return response;
        }
        */
        /// <summary>
        /// Gets shipping rates
        /// </summary>
        /// <param name="shippingOptionRequest">Shipping option request details</param>
        /// <param name="saturdayDelivery">Whether to get rates for Saturday Delivery</param>
        /// <returns>Shipping options; errors if exist</returns>
        /*
        private (IList<ShippingOption> shippingOptions, string error) GetShippingOptions(GetShippingOptionRequest shippingOptionRequest)
        {
            try
            {
                //create request details
                //var request = CreateRateRequest(shippingOptionRequest);

                //get rate response
                //var rateResponse = GetRatesAsync(request).Result;

                //prepare shipping options
                
                return (PrepareShippingOptions(rateResponse).Select(shippingOption =>
                {
                    //correct option name
                    if (!shippingOption.Name.ToLower().StartsWith("ups"))
                        shippingOption.Name = $"UPS {shippingOption.Name}";

                    //add additional handling charge
                    shippingOption.Rate += _shipwireSettings.AdditionalHandlingCharge;

                    return shippingOption;
                }).ToList(), null);
                
            }
            catch (Exception exception)
            {
                //log errors
                var message = $"Error while getting Shipwire rates{Environment.NewLine}{exception.Message}";
                _logger.Error(message, exception, shippingOptionRequest.Customer);

                return (new List<ShippingOption>(), message);
            }
        }
*/

        /// <summary>
        /// Create request details to get shipping rates
        /// </summary>
        /// <param name="shippingOptionRequest">Shipping option request</param>
        /// <param name="saturdayDelivery">Whether to get rates for Saturday Delivery</param>
        /// <returns>Rate request details</returns>
        ///
        
        /*
        private UPSRate.RateRequest CreateRateRequest(GetShippingOptionRequest shippingOptionRequest, bool saturdayDelivery = false)
        {
            //set request details
            var request = new UPSRate.RateRequest
            {
                Request = new UPSRate.RequestType
                {
                    //used to define the request type
                    //Shop - the server validates the shipment, and returns rates for all UPS products from the ShipFrom to the ShipTo addresses
                    RequestOption = new[] { "Shop" }
                }
            };

            //prepare addresses details
            var stateCodeTo = shippingOptionRequest.ShippingAddress.StateProvince?.Abbreviation;
            var stateCodeFrom = shippingOptionRequest.StateProvinceFrom?.Abbreviation;
            var countryCodeFrom = (shippingOptionRequest.CountryFrom ?? _countryService.GetAllCountries().FirstOrDefault())
                .TwoLetterIsoCode ?? string.Empty;

            var addressFromDetails = new UPSRate.ShipAddressType
            {
                AddressLine = new[] { shippingOptionRequest.AddressFrom },
                City = shippingOptionRequest.CityFrom,
                StateProvinceCode = stateCodeFrom,
                CountryCode = countryCodeFrom,
                PostalCode = shippingOptionRequest.ZipPostalCodeFrom
            };
            var addressToDetails = new UPSRate.ShipToAddressType
            {
                AddressLine = new[] { shippingOptionRequest.ShippingAddress.Address1, shippingOptionRequest.ShippingAddress.Address2 },
                City = shippingOptionRequest.ShippingAddress.City,
                StateProvinceCode = stateCodeTo,
                CountryCode = shippingOptionRequest.ShippingAddress.Country.TwoLetterIsoCode,
                PostalCode = shippingOptionRequest.ShippingAddress.ZipPostalCode,
                ResidentialAddressIndicator = string.Empty
            };

            //set shipment details
            request.Shipment = new UPSRate.ShipmentType
            {
                Shipper = new UPSRate.ShipperType
                {
                    ShipperNumber = _upsSettings.AccountNumber,
                    Address = addressFromDetails
                },
                ShipFrom = new UPSRate.ShipFromType
                {
                    Address = addressFromDetails
                },
                ShipTo = new UPSRate.ShipToType
                {
                    Address = addressToDetails
                }
            };

            //set pickup options and customer classification for US shipments
            if (countryCodeFrom.Equals("US", StringComparison.InvariantCultureIgnoreCase))
            {
                request.PickupType = new UPSRate.CodeDescriptionType
                {
                    Code = GetUpsCode(_upsSettings.PickupType)
                };
                request.CustomerClassification = new UPSRate.CodeDescriptionType
                {
                    Code = GetUpsCode(_upsSettings.CustomerClassification)
                };
            }

            //set negotiated rates details
            if (!string.IsNullOrEmpty(_upsSettings.AccountNumber) && !string.IsNullOrEmpty(stateCodeFrom) && !string.IsNullOrEmpty(stateCodeTo))
            {
                request.Shipment.ShipmentRatingOptions = new UPSRate.ShipmentRatingOptionsType
                {
                    NegotiatedRatesIndicator = string.Empty,
                    UserLevelDiscountIndicator = string.Empty
                };
            }

            //set saturday delivery details
            if (saturdayDelivery)
            {
                request.Shipment.ShipmentServiceOptions = new UPSRate.ShipmentServiceOptionsType
                {
                    SaturdayDeliveryIndicator = string.Empty
                };
            }

            //set packages details
            switch (_upsSettings.PackingType)
            {
                case PackingType.PackByOneItemPerPackage:
                    request.Shipment.Package = GetPackagesForOneItemPerPackage(shippingOptionRequest).ToArray();
                    break;

                case PackingType.PackByVolume:
                    request.Shipment.Package = GetPackagesByCubicRoot(shippingOptionRequest).ToArray();
                    break;

                case PackingType.PackByDimensions:
                default:
                    request.Shipment.Package = GetPackagesByDimensions(shippingOptionRequest).ToArray();
                    break;
            }

            return request;
        }
        
        /// <summary>
        /// Get rates
        /// </summary>
        /// <param name="request">Request details</param>
        /// <returns>The asynchronous task whose result contains the rates info</returns>
        private async Task<UPSRate.RateResponse> GetRatesAsync(UPSRate.RateRequest request)
        {
            try
            {
                //create client
                var ratePort = _upsSettings.UseSandbox
                    ? UPSRate.RatePortTypeClient.EndpointConfiguration.RatePort
                    : UPSRate.RatePortTypeClient.EndpointConfiguration.ProductionRatePort;
                using (var client = new UPSRate.RatePortTypeClient(ratePort))
                {
                    //create object to authenticate request
                    var security = new UPSRate.UPSSecurity
                    {
                        ServiceAccessToken = new UPSRate.UPSSecurityServiceAccessToken
                        {
                            AccessLicenseNumber = _upsSettings.AccessKey
                        },
                        UsernameToken = new UPSRate.UPSSecurityUsernameToken
                        {
                            Username = _upsSettings.Username,
                            Password = _upsSettings.Password
                        }
                    };

                    //save debug info
                    if (_upsSettings.Tracing)
                        _logger.Information($"UPS rates. Request: {ToXml(new UPSRate.RateRequest1(security, request))}");

                    //try to get response details
                    var response = await client.ProcessRateAsync(security, request);

                    //save debug info
                    if (_upsSettings.Tracing)
                        _logger.Information($"UPS rates. Response: {ToXml(response)}");

                    return response.RateResponse;
                }
            }
            catch (FaultException<UPSRate.ErrorDetailType[]> ex)
            {
                //get error details
                var message = ex.Message;
                if (ex.Detail.Any())
                {
                    message = ex.Detail.Aggregate(message, (details, detail) =>
                        $"{details}{Environment.NewLine}{detail.Severity} error: {detail.PrimaryErrorCode?.Description}");
                }

                //rethrow exception
                throw new Exception(message, ex);
            }
        }

        /// <summary>
        /// Prepare shipping options
        /// </summary>
        /// <param name="rateResponse">Rate response</param>
        /// <returns>Shipping options</returns>
        private IEnumerable<ShippingOption> PrepareShippingOptions(UPSRate.RateResponse rateResponse)
        {
            var shippingOptions = new List<ShippingOption>();

            if (!rateResponse?.RatedShipment?.Any() ?? true)
                return shippingOptions;

            //prepare offered delivery services
            var servicesCodes = _upsSettings.CarrierServicesOffered.Split(':', StringSplitOptions.RemoveEmptyEntries)
                .Select(idValue => idValue.Trim('[', ']')).ToList();
            var allServices = DeliveryService.Standard.ToSelectList(false).Select(item =>
            {
                var serviceCode = GetUpsCode((DeliveryService)int.Parse(item.Value));
                return new { Name = $"UPS {item.Text?.TrimStart('_')}", Code = serviceCode, Offered = servicesCodes.Contains(serviceCode) };
            }).ToList();

            //get shipping options
            foreach (var rate in rateResponse.RatedShipment)
            {
                //weed out unwanted or unknown service rates
                var serviceCode = rate.Service?.Code;
                var deliveryService = allServices.FirstOrDefault(service => service.Code == serviceCode);
                if (!deliveryService?.Offered ?? true)
                    continue;

                //get rate value
                var regularValue = decimal.TryParse(rate.TotalCharges?.MonetaryValue, out var value) ? (decimal?)value : null;
                var negotiatedValue = decimal.TryParse(rate.NegotiatedRateCharges?.TotalCharge?.MonetaryValue, out value) ? (decimal?)value : null;
                var monetaryValue = negotiatedValue ?? regularValue;
                if (!monetaryValue.HasValue)
                    continue;

                //add shipping option based on service rate
                shippingOptions.Add(new ShippingOption
                {
                    Rate = monetaryValue.Value,
                    Name = deliveryService.Name
                });
            }

            return shippingOptions;
        }
        */
        #endregion
    }
}
