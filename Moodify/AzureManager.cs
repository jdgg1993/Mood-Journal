using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Moodify.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Moodify
{
    public partial class AzureManager
    {

        IMobileServiceSyncTable<Timeline> timelineTable;

        static AzureManager defaultInstance = new AzureManager();
        MobileServiceClient client;

        private AzureManager()
        {
            this.client = new MobileServiceClient(App.ApplicationURL);

            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<Timeline>();

            this.client.SyncContext.InitializeAsync(store);

            this.timelineTable = client.GetSyncTable<Timeline>();
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

        public async Task<ObservableCollection<Timeline>> GetTodoItemsAsync(bool syncItems = false)
        {
            try
            {
                if (syncItems)
                {
                    await this.SyncAsync();
                }
                IEnumerable<Timeline> items = await timelineTable.ToEnumerableAsync();

                return new ObservableCollection<Timeline>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.timelineTable.PullAsync(
                    "allTodoItems",
                    this.timelineTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }
            
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
    }
}
