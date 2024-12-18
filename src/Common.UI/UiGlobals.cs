﻿using System.IO.Abstractions;

using Effanville.Common.Structure.Reporting;
using Effanville.Common.UI.Services;

namespace Effanville.Common.UI
{
    /// <summary>
    /// Class containing services to be used throughout a UI application.
    /// </summary>
    public sealed class UiGlobals
    {
        /// <summary>
        /// The current working directory for the application.
        /// </summary>
        public string CurrentWorkingDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// The current dispatcher for the system.
        /// </summary>
        public IDispatcher CurrentDispatcher
        {
            get;
        }

        /// <summary>
        /// The current filesystem for the application.
        /// </summary>
        public IFileSystem CurrentFileSystem
        {
            get;
        }

        /// <summary>
        /// The interaction mechanism for files, which contains file loading and saving
        /// dialogs.
        /// </summary>
        public IFileInteractionService FileInteractionService
        {
            get;
        }

        /// <summary>
        /// Service for creating Dialog popups.
        /// </summary>
        public IBaseDialogCreationService DialogCreationService
        {
            get;
        }

        /// <summary>
        /// A logging mechanism.
        /// </summary>
        public IReportLogger ReportLogger
        {
            get;
            set;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public UiGlobals(
            string currentWorkingDirectory,
            IDispatcher currentDispatcher,
            IFileSystem currentFileSystem,
            IFileInteractionService fileInteractionService,
            IBaseDialogCreationService dialogCreationService,
            IReportLogger reportLogger)
            : this(
                currentDispatcher,
                currentFileSystem, 
                fileInteractionService,
                dialogCreationService, 
                reportLogger)
        {
            CurrentWorkingDirectory = currentWorkingDirectory;
        }
        
        /// <summary>
        /// Construct an instance.
        /// </summary>
        public UiGlobals(
            IDispatcher currentDispatcher,
            IFileSystem currentFileSystem,
            IFileInteractionService fileInteractionService,
            IBaseDialogCreationService dialogCreationService,
            IReportLogger reportLogger)
        {
            CurrentDispatcher = currentDispatcher;
            CurrentFileSystem = currentFileSystem;
            FileInteractionService = fileInteractionService;
            DialogCreationService = dialogCreationService;
            ReportLogger = reportLogger;
        }
    }
}
