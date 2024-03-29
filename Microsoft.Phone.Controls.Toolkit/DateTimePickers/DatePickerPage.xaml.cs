﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using Microsoft.Phone.Controls.Primitives;

namespace Microsoft.Phone.Controls
{
	/// <summary>
	/// Represents a page used by the DatePicker control that allows the user to choose a date (day/month/year).
	/// </summary>
	public partial class DatePickerPage : DateTimePickerPageBase, IBoundedDateTimePickerPage
	{
		private DateTime? uppperBound;
		private DateTime? lowerBound;

		public DateTime? UpperBound { get { return this.uppperBound; } set { this.uppperBound = value; this.UdateDataSources(); } }
		public DateTime? LowerBound { get { return this.lowerBound; } set { this.lowerBound = value; this.UdateDataSources(); } }

		/// <summary>
		/// Initializes a new instance of the DatePickerPage control.
		/// </summary>
		public DatePickerPage()
		{
			InitializeComponent();

			// Hook up the data sources
			PrimarySelector.DataSource = new YearDataSource();
			SecondarySelector.DataSource = new MonthDataSource();
			TertiarySelector.DataSource = new DayDataSource();

			InitializeDateTimePickerPage(PrimarySelector, SecondarySelector, TertiarySelector);
		}

		private void UdateDataSources()
		{
			((DataSource)PrimarySelector.DataSource).UpperBound = this.UpperBound;
			((DataSource)SecondarySelector.DataSource).UpperBound = this.UpperBound;
			((DataSource)TertiarySelector.DataSource).UpperBound = this.UpperBound;

			((DataSource)PrimarySelector.DataSource).LowerBound = this.LowerBound;
			((DataSource)SecondarySelector.DataSource).LowerBound = this.LowerBound;
			((DataSource)TertiarySelector.DataSource).LowerBound = this.LowerBound;
		}

		/// <summary>
		/// Gets a sequence of LoopingSelector parts ordered according to culture string for date/time formatting.
		/// </summary>
		/// <returns>LoopingSelectors ordered by culture-specific priority.</returns>
		protected override IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern()
		{
			return GetSelectorsOrderedByCulturePattern(
				CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpperInvariant(),
				new char[] { 'Y', 'M', 'D' },
				new LoopingSelector[] { PrimarySelector, SecondarySelector, TertiarySelector });
		}

		/// <summary>
		/// Handles changes to the page's Orientation property.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		protected override void OnOrientationChanged(OrientationChangedEventArgs e)
		{
			if (null == e)
			{
				throw new ArgumentNullException("e");
			}

			base.OnOrientationChanged(e);
			SystemTrayPlaceholder.Visibility = (0 != (PageOrientation.Portrait & e.Orientation)) ?
				Visibility.Visible :
				Visibility.Collapsed;
		}
	}
}
