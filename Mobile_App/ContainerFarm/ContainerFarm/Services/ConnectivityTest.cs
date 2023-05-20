using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Services
{
    public class ConnectivityTest
    {
        public ConnectivityTest()
        {
            // Register for connectivity changes, be sure to unsubscribe when finished
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            throw new AggregateException($"No internet connection. Please connect to the internet.");
        }
    }
}
