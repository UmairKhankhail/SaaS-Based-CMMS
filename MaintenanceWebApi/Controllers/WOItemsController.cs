using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaintenanceWebApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Runtime.CompilerServices;
using System.Text;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;

namespace MaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WOItemsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly ILogger<WOItemsController> _logger;
        public WOItemsController(MaintenanceDbContext context,HttpClient httpClient, JwtTokenHandler jwtTokenHandler, ILogger<WOItemsController> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _JwtTokenHandler= jwtTokenHandler;
            _logger= logger;
        }

        // GET: api/WOItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WOItems>>> GetwOItems()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.wOItems.Where(x => x.companyId ==claimresponse.companyId).ToListAsync();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/WOItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WOItems>> GetWOItems(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var wOItems = await _context.wOItems.Where(x => x.woItemsAutoId == id && x.companyId == claimresponse.companyId).ToListAsync();

                    if (wOItems == null)
                    {
                        return NotFound();
                    }

                    return Ok(wOItems);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // PUT: api/WOItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWOItems(int id,WOItems wOItems)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (wOItems.woItemsAutoId == id && wOItems.companyId == claimresponse.companyId)
                    {
                        _context.Entry(wOItems).State = EntityState.Modified;

                    }


                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!WOItemsExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return NoContent();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/WOItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WOItems>> PostWOItems(WOItems wOItems)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    int getItemsId = 0;
                    var compId = _context.wOItems.Where(i => i.companyId == claimresponse.companyId).Select(d => d.woItemsId).ToList();
                    var autoId = "";
                    if (compId.Count > 0)
                    {

                        autoId = compId.Max(x => int.Parse(x.Substring(1))).ToString();
                    }

                    if (autoId == "")
                    {

                        _context.ChangeTracker.Clear();
                        WOItems items = new WOItems();
                        string comId = "I1";
                        items.itemAutoId = wOItems.itemAutoId;
                        items.woItemsId = comId;
                        items.woAutoId = wOItems.woAutoId;
                        items.itemName = wOItems.itemName;


                        var url = $"https://localhost:7160/api/Equipments/{wOItems.itemAutoId}?companyId={claimresponse.companyId}";

                        var response = await _httpClient.GetAsync(url);
                        var responseContent = await response.Content.ReadAsStringAsync();
                       
                        JObject dataObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                        var stockItems = (int)dataObject["quantity"];

                        items.stock = stockItems;


                        if (stockItems < wOItems.quantity)
                        {
                            var urlPurchase = "https://localhost:7160/api/Purchases";
                            var parameters = new Dictionary<string, object>
                            {
                                { "userAutoId", wOItems.userAutoId },
                                { "status", "pending" },
                                { "purchasesDescp", "Purchase request generated for "+wOItems.itemName },
                                { "companyId", claimresponse.companyId },
                                { "validityCheck", 0 },
                                {
                                    "equipList", new List<Dictionary<string, object>>
                                    {
                                        new Dictionary<string, object>
                                        {
                                            { "equipName", wOItems.itemName },
                                            { "equipQuantity", (wOItems.quantity -stockItems) },
                                            { "equipAutoId", wOItems.itemAutoId }
                                        }
                                    }
                                }
                            };

                            var json = JsonConvert.SerializeObject(parameters);
                            var contentPurchase = new StringContent(json, Encoding.UTF8, "application/json");

                            var responsePurchase = await _httpClient.PostAsync(urlPurchase, contentPurchase);
                            var responsePurchaseContent = await responsePurchase.Content.ReadAsStringAsync();

                          }


                        else if (stockItems >= wOItems.quantity)
                        {
                            var urlIssuence = "https://localhost:7160/api/Issuences";
                            var parametersIssuence = new Dictionary<string, object>
                            {
                                { "userAutoId", wOItems.userAutoId },
                                { "status", "pending" },
                                { "issuenceDescp", "Issuence request generated for "+wOItems.itemName },
                                { "companyId", claimresponse.companyId },
                                { "validityCheck", 0 },
                                {
                                    "equipList", new List<Dictionary<string, object>>
                                    {
                                        new Dictionary<string, object>
                                        {
                                            { "equipName", wOItems.itemName },
                                            { "equipQuantity", wOItems.quantity},
                                            { "equipAutoId", wOItems.itemAutoId }
                                        }
                                    }
                                }
                            };

                            var json = JsonConvert.SerializeObject(parametersIssuence);
                            var contentIssuence = new StringContent(json, Encoding.UTF8, "application/json");

                            var responseIssuence = await _httpClient.PostAsync(urlIssuence, contentIssuence);
                            var responseIssuenceContent = await responseIssuence.Content.ReadAsStringAsync();

                            Console.WriteLine(responseIssuenceContent);


                        }




                        items.quantity = wOItems.quantity;
                        items.cost = (int)dataObject["equipCost"];
                        items.userAutoId = wOItems.userAutoId;
                        items.itemDescp = wOItems.itemDescp;

                        items.requestStatus = wOItems.requestStatus;
                        items.companyId = claimresponse.companyId;
                        _context.wOItems.Add(items);
                        await _context.SaveChangesAsync();
                        getItemsId = items.woItemsAutoId;
                    }

                    if (autoId != "")
                    {
                        _context.ChangeTracker.Clear();
                        WOItems items = new WOItems();
                        string comId = "I" + (int.Parse(autoId) + 1);
                        items.itemAutoId = wOItems.itemAutoId;
                        items.woItemsId = comId;
                        items.woAutoId = wOItems.woAutoId;
                        items.itemName = wOItems.itemName;


                        var url = $"https://localhost:7160/api/Equipments/{wOItems.itemAutoId}?companyId={claimresponse.companyId}";

                        var response = await _httpClient.GetAsync(url);
                        var responseContent = await response.Content.ReadAsStringAsync();
                        
                        JObject dataObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                        var stockItems = (int)dataObject["quantity"];

                        items.stock = stockItems;


                        if (stockItems < wOItems.quantity)
                        {
                            var urlPurchase = "https://localhost:7160/api/Purchases";
                            var parameters = new Dictionary<string, object>
                            {
                                { "userAutoId", wOItems.userAutoId },
                                { "status", "pending" },
                                { "purchasesDescp", "Purchase request generated for "+wOItems.itemName },
                                { "companyId", claimresponse.companyId },
                                { "validityCheck", 0 },
                                {
                                    "equipList", new List<Dictionary<string, object>>
                                    {
                                        new Dictionary<string, object>
                                        {
                                            { "equipName", wOItems.itemName },
                                            { "equipQuantity", (wOItems.quantity -stockItems) },
                                            { "equipAutoId", wOItems.itemAutoId }
                                        }
                                    }
                                }
                            };

                            var json = JsonConvert.SerializeObject(parameters);
                            var content = new StringContent(json, Encoding.UTF8, "application/json");

                            var responsePurchase = await _httpClient.PostAsync(url, content);
                            var responsePurchaseContent = await response.Content.ReadAsStringAsync();

                            Console.WriteLine(responsePurchaseContent);


                        }

                        else if (stockItems >= wOItems.quantity)
                        {
                            var urlIssuence = "https://localhost:7160/api/Issuences";
                            var parametersIssuence = new Dictionary<string, object>
                            {
                                { "userAutoId", wOItems.userAutoId },
                                { "status", "pending" },
                                { "issuenceDescp", "Issuence request generated for "+wOItems.itemName },
                                { "companyId", claimresponse.companyId },
                                { "validityCheck", 0 },
                                {
                                    "equipList", new List<Dictionary<string, object>>
                                    {
                                        new Dictionary<string, object>
                                        {
                                            { "equipName", wOItems.itemName },
                                            { "equipQuantity", wOItems.quantity},
                                            { "equipAutoId", wOItems.itemAutoId }
                                        }
                                    }
                                }
                            };

                            Console.WriteLine(parametersIssuence);
                            var json = JsonConvert.SerializeObject(parametersIssuence);
                            var contentIssuence = new StringContent(json, Encoding.UTF8, "application/json");

                            var responseIssuence = await _httpClient.PostAsync(urlIssuence, contentIssuence);
                            var responseIssuenceContent = await responseIssuence.Content.ReadAsStringAsync();

                            Console.WriteLine(responseIssuenceContent);


                        }


                        items.quantity = wOItems.quantity;
                        items.cost = (int)dataObject["equipCost"];
                        items.userAutoId = wOItems.userAutoId;
                        items.itemDescp = wOItems.itemDescp;
                        items.requestStatus = wOItems.requestStatus;
                        items.companyId = claimresponse.companyId;
                        _context.wOItems.Add(items);
                        await _context.SaveChangesAsync();
                        getItemsId = items.woItemsAutoId;
                    }

                    return Ok();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // DELETE: api/WOItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWOItems(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var wOItems = await _context.wOItems.FindAsync(id);
                    if (wOItems == null)
                    {
                        return NotFound();
                    }

                    _context.wOItems.Remove(wOItems);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        private bool WOItemsExists(int id)
        {
            return _context.wOItems.Any(e => e.woItemsAutoId == id);
        }
    }
}
