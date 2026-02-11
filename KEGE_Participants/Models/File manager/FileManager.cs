using System.IO;
using Task_Data;
using System.Diagnostics;
using KEGE_Participants.Windows;

namespace KEGE_Participants.Models.File_manager
{
    public class FileManager
    {
        public void OpenFile(FileData file)
        {
            if (file == null || file.Data == null) return;

            try
            {
                // Создаем путь во временной директории пользователя
                string tempFolder = Path.Combine(Path.GetTempPath(), "EgeClient_Cache");
                if (!Directory.Exists(tempFolder)) Directory.CreateDirectory(tempFolder);

                string filePath = Path.Combine(tempFolder, file.FileName);

                // Записываем данные в файл 
                File.WriteAllBytes(filePath, file.Data);

                // Открываем файл программой по умолчанию
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true // Это заставит Windows искать сопоставленное приложение
                });
            }
            catch (Exception ex)
            {
                NotificationWindow.QuickShow("Ошибка при открытии файла", ex.Message, NotificationType.Error);
            }
        }

        public string GetIconPath(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();

            return extension switch
            {
                ".txt" => "/Resources/Extensions/txt96x96.png",
                ".odt" or ".doc" or ".docx" => "/Resources/Extensions/word96x96.png",
                ".ods" or ".xls" or ".xlsx" => "/Resources/Extensions/exel96x96.png",
                _ => "/Resources/Extensions/unknownFile96xx96.png"
            };
        }
    }
}
