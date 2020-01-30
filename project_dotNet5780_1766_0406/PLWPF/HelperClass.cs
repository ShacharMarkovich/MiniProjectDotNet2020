using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PLWPF
{
    static class HelperClass
    {
        public static bool Contains(this BE.BankBranch branch, string word)
        {
            if (branch.BankNumber.ToString().Contains(word))
                return true;
            if (branch.BankName.Contains(word))
                return true;
            if (branch.BranchAddress.Contains(word))
                return true;
            if (branch.BranchCity.Contains(word))
                return true;
            if (branch.BranchNumber.ToString().Contains(word))
                return true;

            return false;
        }

        public static string AsString(this BE.BankBranch bank)
        {
            return $"{bank.BankNumber} {bank.BranchNumber} {bank.BankName} {bank.BranchCity} {bank.BranchAddress}";
        }
        public static void Fade(this TextBlock txtBlock)
        {
            txtBlock.Background = Brushes.AliceBlue;
            txtBlock.Opacity = 1;
            DoubleAnimation hold = new DoubleAnimation()
            {
                From = 1,
                To = 1,
                Duration = TimeSpan.FromSeconds(2)
            };
            DoubleAnimation fade = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(2)
            };

            fade.Completed += (sender, args) => txtBlock.Opacity = 0;
            hold.Completed += (sender, args) => txtBlock.BeginAnimation(Label.OpacityProperty, fade);
            txtBlock.BeginAnimation(Label.OpacityProperty, hold);
        }
    }
}
