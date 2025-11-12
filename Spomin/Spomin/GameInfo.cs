using System.Windows.Forms;

namespace Spomin
{
    public static class GameInfo
    {
        public static void ShowRules()
        {
            string message = "Memory igra s kartami:\n\n" +
                             "1. Klikni karto, da jo obrneš.\n" +
                             "2. Obrni dve karti, da najdeš ujemajoči par.\n" +
                             "3. Če se karti ujemata, ostaneta obrnjeni.\n" +
                             "4. Če se ne ujemata, se po 1 sekundi obrnejo nazaj.\n" +
                             "5. Igra se konča, ko so vse karte ujemajoče.\n" +
                             "6. Cilj je najti čim več parov.";

            MessageBox.Show(message, "Kako igrati", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
