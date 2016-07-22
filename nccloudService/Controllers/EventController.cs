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
    public class EventController : TableController<Event>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            nccloudContext context = new nccloudContext();
            DomainManager = new EntityDomainManager<Event>(context, Request);
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

        // GET tables/Event
        public async Task<IQueryable<Event>> GetAllEvent()
        {
            var emailAddr = await GetEmailAddress();

            return Query().Where(i=>i.Location.Custommers.All(item=>item.CustomerEmail == emailAddr)); 
        }

        // GET tables/Event/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Event> GetEvent(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Event/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Event> PatchEvent(string id, Delta<Event> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Event
        public async Task<IHttpActionResult> PostEvent(Event item)
        {
            Event current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Event/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteEvent(string id)
        {
             return DeleteAsync(id);
        }
    }
}
