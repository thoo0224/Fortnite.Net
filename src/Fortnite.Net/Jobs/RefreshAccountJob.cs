using Quartz;

using System;
using System.Threading.Tasks;

namespace Fortnite.Net.Jobs
{
    public class RefreshAccountJob : IJob
    {

        public async Task Execute(IJobExecutionContext context)
        {
            if (!(context.MergedJobDataMap["client"] is FortniteApiClient client))
            {
                throw new InvalidOperationException("Tried to execute refresh token job but client was null or invalid type.");

            }

            var response = await client.AccountPublicService.RefreshAccessTokenAsync();
            if (response.IsSuccessful)
            {
                await client.OnRefreshAsync(response.Data);
            }
            else
            {
                throw new InvalidOperationException($"Failed to refresh account. ({response.Error?.Error})");
            }
        }

    }
}
