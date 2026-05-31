using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WayPoint.Services
{
    public static class SoundHelper
    {
        // Підключаємо системну бібліотеку Windows для відтворення без затримок
        [DllImport("winmm.dll", SetLastError = true)]
        private static extern bool PlaySound(string pszSound, IntPtr hmod, uint fdwSound);

        private const uint SND_FILENAME = 0x00020000;
        private const uint SND_ASYNC = 0x0001; // Асинхронно, щоб не гальмувати програму

        public static void PlayClick()
        {
            try
            {
                // Миттєве відтворення звуку Windows
                PlaySound(@"C:\Windows\Media\Windows Navigation Start.wav", IntPtr.Zero, SND_FILENAME | SND_ASYNC);
            }
            catch { }
        }

        // Рекурсивно вішає звук на всі кнопки форми
        public static void AttachSounds(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                // Якщо це кнопка АБО наші кастомні хрестики/назад
                if (c is Button || (c is Label lbl && (lbl.Name.Contains("Exit") || lbl.Name.Contains("Back"))))
                {
                    // Відписуємось, щоб випадково не повісити звук двічі, і підписуємось
                    c.Click -= HandleClick;
                    c.Click += HandleClick;
                }

                if (c.HasChildren)
                {
                    AttachSounds(c);
                }
            }
        }

        private static void HandleClick(object sender, EventArgs e)
        {
            PlayClick();
        }
    }
}