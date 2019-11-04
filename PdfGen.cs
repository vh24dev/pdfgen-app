using System.Collections;
using Aspose.Words;
using Aspose.Words.Fonts;

namespace dotnet_pdf {
    internal class PdfGenException: System.Exception {
        internal PdfGenException(string message): base($"Failed to generate PDF from DOCX: {message}") {}
    }
    internal class PdfGen {
        static PdfGen() => LoadAposeLicense();

        internal static void DocxToPdf(string inPath, string outPath, string[] mergefields, string[] mergefieldData) {
            // Load the document from disk.
            try {
                Document doc = new Document(inPath);
                if (mergefields != null && mergefieldData != null) {
                    if (mergefields.Length != mergefieldData.Length) {
                        throw new PdfGenException("mailmerge expects mergefields and mergefieldData to have the same length");
                    }

                    if (mergefields.Length > 0) {
                        doc.MailMerge.Execute(mergefields, mergefieldData);
                    } 
                }
                doc.Save(outPath);
            } catch (System.Exception e) {
                throw new PdfGenException(e.Message);
            }
        }
        internal static void LoadCustomFonts(string path) {
            // Add the custom folder which contains our fonts to the list of existing font sources.
            ArrayList fontSources = new ArrayList(FontSettings.DefaultInstance.GetFontsSources());
            FolderFontSource folderFontSource = new FolderFontSource(path, true);
            fontSources.Add(folderFontSource);
            FontSourceBase[] updatedFontSources = (FontSourceBase[]) fontSources.ToArray(typeof(FontSourceBase));
            // "Setting this property resets the cache of all previously loaded fonts."
            FontSettings.DefaultInstance.SetFontsSources(updatedFontSources);
        }

        private const string ASPOSE_LICENSE_FILE = "Aspose.Words.lic";
        private static void LoadAposeLicense() {
            // Attempt to set a license from several locations relative to the executable and Aspose.Words.dll.
            try {
                var license = new Aspose.Words.License();
                license.SetLicense(ASPOSE_LICENSE_FILE);
                System.Console.WriteLine("Aspose license set successfully");
            } catch (System.Exception e) {
                System.Console.WriteLine($"Error setting Aspose license: {e.Message}");
            }
        }
    }
}