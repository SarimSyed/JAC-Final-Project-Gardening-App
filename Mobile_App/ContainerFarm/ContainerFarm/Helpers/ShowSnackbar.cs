using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Helpers
{
    public class ShowSnackbar
    {
        public async static void NewSnackbar(string message)
        {
            // Snackbar options
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = new Color(121, 216, 99),
                CornerRadius = new CornerRadius(10),
            };

            // Show successful snackbar message
            ISnackbar snackbar = Snackbar.Make(
                message,
                null,
                "OK",
                null,
                snackbarOptions);

            // Show snackbar
            await snackbar.Show();
        }
    }
}
