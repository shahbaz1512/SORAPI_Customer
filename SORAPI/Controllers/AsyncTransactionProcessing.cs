using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using SORAPI.Classes;
using SORAPI.Interface;
using System.Net.Http;

namespace SORAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsyncTransactionProcessing : ControllerBase
    {
        private readonly ProcessAsyncTransactionInterface _ProcessAsyncTransactionInterface;
        private readonly Datavalidator _datavalidator;
        // _ibbpsInterface interface is injected dependency in controller
        public AsyncTransactionProcessing(ProcessAsyncTransactionInterface TransactionInterfaceInterface, Datavalidator datavalidator)  //////Constructor class  with dependncy injection
        {
            _ProcessAsyncTransactionInterface = TransactionInterfaceInterface ?? throw new ArgumentNullException(nameof(TransactionInterfaceInterface));
            _datavalidator = datavalidator ?? throw new ArgumentException(nameof(TransactionInterfaceInterface));
        }
        Request _request = new Request();
        Response _response = new Response();

        [HttpPost("CustomerOnboard")]
        public async Task<IActionResult> CustomerOnboard()
        {
            try
            {
                var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                requestBody = await _datavalidator.SSLDecrypt(requestBody);
                _request = JsonConvert.DeserializeObject<Request>(requestBody);
                Log.Information("CustomerOnboard received: {RequestBody}", _request.CustomerId);
                await _datavalidator.DataValidators(enumTransactionType.CustomerOnboard, _request);
                _response = await _ProcessAsyncTransactionInterface.InsertCustomerInformation(_request);
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during processing.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during processing.");
            }
        }

        [HttpPost("CustomerTransaction")]
        public async Task<IActionResult> CustomerTransaction()
        {
            try
            {
                var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                requestBody = await _datavalidator.SSLDecrypt(requestBody);
                _request = JsonConvert.DeserializeObject<Request>(requestBody);
                Log.Information("CustomerTransaction received: {RequestBody}", _request.CustomerId);
                await _datavalidator.DataValidators(enumTransactionType.CustomerOnboard, _request);
                _response = await _ProcessAsyncTransactionInterface.InsertCustomerInformation(_request);
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during processing.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during processing.");
            }
        }

        [HttpPost("ChkCustomerDebitTxn")]
        public async Task<IActionResult> ChkCustomerDebitTxn()
        {
            try
            {
                var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                requestBody = await _datavalidator.SSLDecrypt(requestBody);
                _request = JsonConvert.DeserializeObject<Request>(requestBody);
                Log.Information("ChkCustomerDebitTxn received: {RequestBody}", _request.CustomerId);
                await _datavalidator.DataValidators(enumTransactionType.CustomerOnboard, _request);
                _response = await _ProcessAsyncTransactionInterface.InsertCustomerInformation(_request);
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during processing.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during processing.");
            }
        }

        [HttpPost("ChkCustomerCreditTxn")]
        public async Task<IActionResult> ChkCustomerCreditTxn()
        {
            try
            {
                var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                requestBody = await _datavalidator.SSLDecrypt(requestBody);
                _request = JsonConvert.DeserializeObject<Request>(requestBody);
                Log.Information("ChkCustomerCreditTxn received: {RequestBody}", _request.CustomerId);
                await _datavalidator.DataValidators(enumTransactionType.CustomerOnboard, _request);
                _response = await _ProcessAsyncTransactionInterface.InsertCustomerInformation(_request);
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during processing.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during processing.");
            }
        }

        [HttpPost("CustomerTopup")]
        public async Task<IActionResult> CustomerTopup()
        {
            try
            {
                var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                requestBody = await _datavalidator.SSLDecrypt(requestBody);
                _request = JsonConvert.DeserializeObject<Request>(requestBody);
                Log.Information("CustomerTopup received: {RequestBody}", _request.CustomerId);
                await _datavalidator.DataValidators(enumTransactionType.CustomerOnboard, _request);
                _response = await _ProcessAsyncTransactionInterface.InsertCustomerInformation(_request);
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during processing.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during processing.");
            }
        }

        [HttpPost("CustomerReversal")]
        public async Task<IActionResult> CustomerReversal()
        {
            try
            {
                var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                requestBody = await _datavalidator.SSLDecrypt(requestBody);
                _request = JsonConvert.DeserializeObject<Request>(requestBody);
                Log.Information("CustomerReversal received: {RequestBody}", _request.CustomerId);
                await _datavalidator.DataValidators(enumTransactionType.CustomerOnboard, _request);
                _response = await _ProcessAsyncTransactionInterface.InsertCustomerInformation(_request);
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during processing.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during processing.");
            }
        }

        [HttpPost("CustomerValidation")]
        public async Task<IActionResult> CustomerValidation()
        {
            try
            {
                var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                requestBody = await _datavalidator.SSLDecrypt(requestBody);
                _request = JsonConvert.DeserializeObject<Request>(requestBody);
                Log.Information("CustomerValidation received: {RequestBody}", _request.CustomerId);
                await _datavalidator.DataValidators(enumTransactionType.CustomerOnboard, _request);
                _response = await _ProcessAsyncTransactionInterface.InsertCustomerInformation(_request);
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during processing.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during processing.");
            }
        }

        [HttpPost("GetCustomerDetails")]
        public async Task<IActionResult> GetCustomerDetails()
        {
            try
            {
                var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                requestBody = await _datavalidator.SSLDecrypt(requestBody);
                _request = JsonConvert.DeserializeObject<Request>(requestBody);
                Log.Information("CustomerValidation received: {RequestBody}", _request.CustomerId);
                await _datavalidator.DataValidators(enumTransactionType.CustomerOnboard, _request);
                _response = await _ProcessAsyncTransactionInterface.InsertCustomerInformation(_request);
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during processing.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during processing.");
            }
        }

        [HttpPost("UpdateCustomersDetails")]
        public async Task<IActionResult> UpdateCustomersDetails()
        {
            try
            {
                var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                requestBody = await _datavalidator.SSLDecrypt(requestBody);
                _request = JsonConvert.DeserializeObject<Request>(requestBody);
                Log.Information("CustomerValidation received: {RequestBody}", _request.CustomerId);
                await _datavalidator.DataValidators(enumTransactionType.CustomerOnboard, _request);
                _response = await _ProcessAsyncTransactionInterface.InsertCustomerInformation(_request);
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during processing.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during processing.");
            }
        }
        [HttpPost("BlockCustomers")]
        public async Task<IActionResult> BlockCustomers()
        {
            try
            {
                var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                requestBody = await _datavalidator.SSLDecrypt(requestBody);
                _request = JsonConvert.DeserializeObject<Request>(requestBody);
                Log.Information("CustomerValidation received: {RequestBody}", _request.CustomerId);
                await _datavalidator.DataValidators(enumTransactionType.CustomerOnboard, _request);
                _response = await _ProcessAsyncTransactionInterface.InsertCustomerInformation(_request);
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during processing.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during processing.");
            }
        }
        [HttpPost("UnBlockCustomers")]
        public async Task<IActionResult> UnBlockCustomers()
        {
            try
            {
                var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                requestBody = await _datavalidator.SSLDecrypt(requestBody);
                _request = JsonConvert.DeserializeObject<Request>(requestBody);
                Log.Information("CustomerValidation received: {RequestBody}", _request.CustomerId);
                await _datavalidator.DataValidators(enumTransactionType.CustomerOnboard, _request);
                _response = await _ProcessAsyncTransactionInterface.InsertCustomerInformation(_request);
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during processing.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during processing.");
            }
        }

    }
}

