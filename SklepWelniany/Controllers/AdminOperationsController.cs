using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SklepWelniany.Constants;

namespace SklepWelniany.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class AdminOperationsController : Controller
    {
        private readonly IUserOrderRepository _userOrderRepository;
        public AdminOperationsController(IUserOrderRepository userOrderRepository)
        {
            _userOrderRepository = userOrderRepository;
        }

        public async Task<IActionResult> AllOrders()
        {
            var orders = await _userOrderRepository.UserOrders(true);
            return View(orders);
        }

        public async Task<IActionResult> TogglePaymentStatus(int orderId)
        {
            try
            {
                await _userOrderRepository.TogglePaymentStatus(orderId);
            }
            catch (Exception ex)
            {
                // Log the exception 
            }
            return RedirectToAction(nameof(AllOrders));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateOrderStatus(int orderId)
        {
            var order = await _userOrderRepository.GetOrderById(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {orderId} not found.");
            }
            var orderStatusList = (await _userOrderRepository.GetOrderStatuses())
                .Select(orderStatus =>
                {
                    return new SelectListItem
                    {
                        Value = orderStatus.Id.ToString(), //value of dropdown list
                        Text = orderStatus.StatusName, // displayed text in list
                        Selected = order.OrderStatusId == orderStatus.Id // selected text in list
                    };
                });

            var data = new UpdateOrderStatusModel
            {
                OrderId = order.Id,
                OrderStatusId = order.OrderStatusId,
                OrderStatusList = orderStatusList
            };
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatusModel data)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    data.OrderStatusList = (await _userOrderRepository.GetOrderStatuses())
                        .Select(orderStatus =>
                        {
                            return new SelectListItem
                            {
                                Value = orderStatus.Id.ToString(), //value of dropdown list
                                Text = orderStatus.StatusName, // displayed text in list
                                Selected = orderStatus.Id == data.OrderStatusId // selected text in list
                            };
                        });
                    TempData["msg"] = "Błąd walidacji danych! Sprawdź komunikaty na czerwo.";
                    return View(data);
                }
                await _userOrderRepository.ChangeOrderStatus(data);
                TempData["msg"] = "Order status updated successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception 
                TempData["msg"] = $"Wystąpił błąd {ex.Message}";
            }
            return RedirectToAction(nameof(UpdateOrderStatus), new { orderId = data.OrderId });
        }

    }

}
