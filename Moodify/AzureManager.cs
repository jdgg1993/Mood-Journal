using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moodify
{
    public partial class AzureManager
    {

        static AzureManager defaultInstance = new AzureManager();
        MobileServiceClient client;

        private AzureManager()
        {
            this.client = new MobileServiceClient(App.ApplicationURL);
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public static AzureManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }
    }
}
