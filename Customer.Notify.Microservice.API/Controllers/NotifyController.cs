using Customer.Notify.Microservice.API.Services;
using Customer.Notify.Microservice.APP;
using Customer.Notify.Microservice.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Customer.Notify.Microservice.API.Controllers
{
    [ApiController]
    [Route("Customer.Notify.Microservice.API.Notify")]
    public class NotifyController : Controller
    {

        private readonly INotifyRepository _notifierRepository;
        private readonly RabbitMQConsumer _rabbitMQConsumer;
        public NotifyController(INotifyRepository r, RabbitMQConsumer c)
        {
            _notifierRepository = r;
            _rabbitMQConsumer = c;

        }

        [HttpGet]
        [Route("AllNotifications")]
        public ActionResult<List<Notification>> AllNotifications()
        {
            try
            {
                var result = _notifierRepository.AllNotifications();

                return Ok(result);


            }catch(Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPost]
        [Route("AllNotificationsByID")]
        public ActionResult<List<Notification>> AllNotificationByID(int id)
        {
            try
            {
                var result = _notifierRepository.AllNotificationsByID(id);

                return Ok(result);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete]
        [Route("DeleteANotificationById")]
        public async Task<ActionResult<string>> DeleteANotificationByID(int id)
        {
            try
            {
                var result = await _notifierRepository.DeleteByID(id);

                return Ok(result);


            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SendACreationAccountNotification")]
        public async Task<ActionResult<string>> sendAnAccountCreationNotification(string email, string text_message, int id)
        {
            try
            {
                var result = await _notifierRepository.SendAccountCreationNotification(email,text_message,id);
                var rabbitMqProducer = new RabbitMQProducer();
                await rabbitMqProducer.NotifyAccountCreationEmailSent(email, id);

                return Ok(result);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("SendARemoveAccountNotification")]
        public async  Task<ActionResult<string>> SendARemoveAccountNotification(string email, string text_message, int id)
        {
            try
            {
                var result = await _notifierRepository.SendAccountRemovedNotification(email,text_message,id);
                var rabbitMqProducer = new RabbitMQProducer();
                await rabbitMqProducer.NotifyAccountRemovedEmailSent(email, id);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("SendAnUpdatedAccountNotification")]
        public async Task<ActionResult<string>> SendAUpdatedAccountNotification(string email, string text_message, int id)
        {
            try
            {
                var result = await _notifierRepository.SendUpdateAccountNotification(email, text_message, id);
                var rabbitMqProducer = new RabbitMQProducer();
                await rabbitMqProducer.NotifyAccountUpdatedEmailSent(email, id);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("SendDeploymentNotification")]
        public async Task<ActionResult<string>> SendDeploymentNotification(string email, string text_message, int id)
        {
            try
            {
                var result = await _notifierRepository.SendDeploymentNotification(email,text_message,id);
                var rabbitMqProducer = new RabbitMQProducer();
                await rabbitMqProducer.NotifyAppDeployedEmailSent(email, id);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("SendRecoveyPassNotification")]
        public async Task<ActionResult<string>> SendRecoveyPassNotification(string email, string text_message, int id)
        {
            try
            {
                var result = await _notifierRepository.SendRecoveyPassNotification(email, text_message, id);

                _ = Task.Run(() => _rabbitMQConsumer.StartListening());

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SendRemoveDeploymentNotification")]
        public async Task<ActionResult<string>> SendRemoveDeploymentNotification(string email, string text_message, int id)
        {
            try
            {
                var result = await _notifierRepository.SendRemovedDeploymentNotification(email, text_message, id);

                var rabbitMqProducer = new RabbitMQProducer();
                await rabbitMqProducer.NotifyRemovedDeploymentEmailSent(email, id);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("SendInfrastructureNotification")]
        public async Task<ActionResult<string>> SendInfrastructureNotification(string email, string text_message, int id)
        {
            try
            {
                var result = await _notifierRepository.SendInfrastructureNotification(email, text_message, id);

                var rabbitMqProducer = new RabbitMQProducer();
                await rabbitMqProducer.NotifyInfrastructureEmailSent(email, id);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
