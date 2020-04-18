using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.Build.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using PasteProperty.Common;
using PasteProperty.Common.Extentions;
using PasteProperty.ValueRepository;
using Task = System.Threading.Tasks.Task;

namespace PasteProperty
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class PastePrivateFieldCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4141;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("bf5a8902-3958-497d-b07a-2dae843b50b1");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;


        private IValueRepository _valueRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertSelectedToFieldCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private PastePrivateFieldCommand(AsyncPackage package, OleMenuCommandService commandService, IValueRepository valueRepository)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
            _valueRepository = valueRepository;
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static PastePrivateFieldCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package, IValueRepository valueRepository)
        {
            // Switch to the main thread - the call to AddCommand in ConvertSelectedToFieldCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new PastePrivateFieldCommand(package, commandService, valueRepository);       
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            var value = _valueRepository.GetMainValue().ToPrivateField();

            DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));

            var selection = (TextSelection)dte.ActiveDocument.Selection;
            selection.Text = selection.Text + value;         
        }
    }
}
