﻿using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace PasteProperty
{

    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PastePropertyPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]

    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class PastePropertyPackage : AsyncPackage
    {
        /// <summary>
        /// PastePropertyPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "eeeb753f-fb7e-40e0-9170-9fa0fad39327";

        #region Package Members

        ValueRepository _valueRepository = new ValueRepository();


        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {

            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await ConvertSelectedToPropertyCommand.InitializeAsync(this);
            await ConvertSelectedToFieldCommand.InitializeAsync(this);
            await ConvertSelectedToPrivateFieldCommand.InitializeAsync(this);
            await CopyValue1Command.InitializeAsync(this, _valueRepository);
            await CopyValue2Command.InitializeAsync(this, _valueRepository);
            await CopyValue3Command.InitializeAsync(this, _valueRepository);

            await PasteFieldCommand.InitializeAsync(this, _valueRepository);
            
            await PastePrivateFieldCommand.InitializeAsync(this, _valueRepository);
            await PastePropertyCommand.InitializeAsync(this, _valueRepository);

            await SelectValue1Command.InitializeAsync(this, _valueRepository);
            await SelectValue2Command.InitializeAsync(this, _valueRepository);
            await SelectValue3Command.InitializeAsync(this, _valueRepository);
        }



        #endregion
    }
}
