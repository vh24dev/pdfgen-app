using System;
using System.Linq;

namespace dotnet_pdf {
    class Program {
        private const string CUSTOM_FONTS_PATH_ENV_VAR = "PDFGEN_CUSTOM_FONT_PATH";
        private const string CUSTOM_FONTS_PATH_DEFAULT = "./pdfgen/assets/custom_fonts";
        static int Main(string[] args) {
            Console.WriteLine("--- docx to pdf converter ---\n\n");
            string[] bla = new string[]{"a", "b"};
            if (args.Length < 1) {
                Console.WriteLine("Path to docx doc is mandatory\n");
                printUsage();

                return 1;
            }

            // Mail merge
            var mmIdx = Array.FindIndex(args, a => a == "--mailmerge");
            string[] mergefields = null;
            string[] mergefieldData = null;
            if (mmIdx >= 1) {
                if (args.Length != mmIdx + 3) {
                    Console.WriteLine("Failed to use mailmerge: invalid number of params.");
                    printUsage();
                    return 1;
                }

                Console.WriteLine("Using mailmerge.");
                mergefields = args[mmIdx + 1].Split(";"); // can break
                mergefieldData = args[mmIdx + 2].Split(";");
            }
            //--

            // Custom fonts
            var cfPath = System.Environment.GetEnvironmentVariable(CUSTOM_FONTS_PATH_ENV_VAR);
            if (String.IsNullOrWhiteSpace(cfPath)) {
                Console.WriteLine($"Custom fonts env var is empty, using default path");
                cfPath = CUSTOM_FONTS_PATH_DEFAULT;
            }

            Console.WriteLine($"Loading custom fonts from: {cfPath}");
            PdfGen.LoadCustomFonts(cfPath);
            //--

            // Removing empty args. Workaround for having our other docgen tool feeding us an empty first arg.
            var cleanArgs = args.Where(a => !String.IsNullOrWhiteSpace(a))
                                     .ToList();
            var docxPath = cleanArgs[0];
            string outPath = cleanArgs.Count == 2 ? cleanArgs[1] : System.IO.Path.ChangeExtension(docxPath, "pdf");
            Console.WriteLine($"Calling DocxToPdf with docx: {docxPath} pdf: {outPath}");
            PdfGen.DocxToPdf(docxPath, outPath, mergefields, mergefieldData);

            Console.WriteLine("Done");

            return 0;
        }

        private static void printUsage() {
            Console.WriteLine(
$@"Usage: dotnet_pdf <path to docx> [path to pdf] [--mailmerge <mergefields> <field data>].
Writes to ./<docx filename>.pdf if no <path to pdf> is provided as second argument provided. 

When using --mailmerge, <mergefields> and <field data> are each a list of fields separated by ';'. Both are expected to have the same size.

Example:
dotnet_pdf ../vollmachts/DG_Krutisch_vollmacht_Allgemein\ 1.8.docx
dotnet_pdf ../vollmachts/DG_Krutisch_vollmacht_Allgemein\ 1.8.docx --mailmerge ""Contact.FirstName;Vehicle_Brand__c.Name"" ""bla;ble""

Note: Use env var {CUSTOM_FONTS_PATH_ENV_VAR} for custom fonts.");
        }
    }
}
