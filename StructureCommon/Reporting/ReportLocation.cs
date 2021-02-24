namespace StructureCommon.Reporting
{
    /// <summary>
    /// Details a location pertaining to a report.
    /// </summary>
    public enum ReportLocation
    {
        /// <summary>
        /// The default, an unknown location.
        /// </summary>
        Unknown,

        /// <summary>
        /// In an area accessing the internet to get information.
        /// </summary>
        Downloading,

        /// <summary>
        /// In an area when saving information to disc.
        /// </summary>
        Saving,
		
        /// <summary>
        /// In an area loading a file or some information from disc.
        /// </summary>
        Loading,

        /// <summary>
        /// In an area adding data to the data store.
        /// </summary>
        AddingData,

        /// <summary>
        /// In an area editing data to the data store.
        /// </summary>
        EditingData,

        /// <summary>
        /// In an area deleting data to the data store.
        /// </summary>
        DeletingData,

        /// <summary>
        /// In an area when interpreting user text input.
        /// </summary>
        Parsing,

        /// <summary>
        /// An area pertaining to calcluating statistics.
        /// </summary>
        StatisticsGeneration,

        /// <summary>
        /// An area to do with getting data from a data store.
        /// </summary>
        DatabaseAccess,

        /// <summary>
        /// In an area to do with help notifications.
        /// </summary>
        Help,
		
		/// <summary>
		/// An area to do with data alteration.
		/// </summary>
		DataAlteration,
         
		/// <summary>
        /// An area pertaining to calcluating statistics.
        /// </summary>
        StatisticsGeneration,
		
		/// <summary>
		/// An area pertaining to execution.
		/// </summary>
		Execution
	}
}
