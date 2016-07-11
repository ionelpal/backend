using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using nccloudService.DataObjects;
using nccloudService.Models;
using System.Security.Principal;
using Microsoft.Azure.Mobile.Server.Authentication;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace nccloudService.Controllers
{
    [Authorize]
    public class MessageController : TableController<Message>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            nccloudContext context = new nccloudContext();
            DomainManager = new EntityDomainManager<Message>(context, Request);
        }
        //get user email address
        private string GoogleSID()
        {
            var principal = this.User as ClaimsPrincipal;
            var sid = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            return sid;
        }
        private async Task<string> GetEmailAddress()
        {
            var credentials = await User.GetAppServiceIdentityAsync<GoogleCredentials>(Request);
            return credentials.UserClaims
                .Where(claim => claim.Type.EndsWith("/emailaddress"))
                .First<Claim>()
                .Value;
            // Debug.WriteLine(credentials.UserClaims);
        }

        // GET tables/Message
        public async Task<IQueryable<Message>> GetAllMessage()
        {
          //  Debug.WriteLine("GET tables/TodoItem");
            var emailAddr = await GetEmailAddress();
            return Query().Where(item => item.CustomerEmail == emailAddr);
        }

        // GET tables/Message/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<SingleResult<Message>> GetMessage(string id)
        {
           // Debug.WriteLine($"GET tables/Message/{id}");
            var emailAddr = await GetEmailAddress();
            var result = Lookup(id).Queryable.Where(item => item.CustomerEmail == emailAddr);
            return new SingleResult<Message>(result);
        }

        // PATCH tables/Message/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<Message> PatchMessage(string id, Delta<Message> patch)
        {
           // Debug.WriteLine($"PATCH tables/TodoItem/{id}");
            var item = Lookup(id).Queryable.FirstOrDefault<Message>();
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            var emailAddr = await GetEmailAddress();
            if (item.CustomerEmail != emailAddr)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            return await UpdateAsync(id, patch);
        }

        // POST tables/Message
        public async Task<IHttpActionResult> PostMessage(Message item)
        {
          //  Debug.WriteLine($"POST tables/TodoItem");
            var emailAddr = await GetEmailAddress();
            item.CustomerEmail = emailAddr;
            Message current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Message/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task DeleteMessage(string id)
        {
           // Debug.WriteLine($"DELETE tables/TodoItem/{id}");
            var item = Lookup(id).Queryable.FirstOrDefault<Message>();
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            var emailAddr = await GetEmailAddress();
            if (item.CustomerEmail != emailAddr)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            await DeleteAsync(id);
            // return DeleteAsync(id);
        }
    }
}
