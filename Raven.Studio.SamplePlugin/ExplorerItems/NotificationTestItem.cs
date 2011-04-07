namespace Raven.Studio.SamplePlugin.ExplorerItems
{
	using System.ComponentModel.Composition;
	using Caliburn.Micro;
	using Messages;
	using Plugins.Database;

	[ExportDatabaseExplorerItem("Notifications", Index = 100)]
	public class NotificationTestItem : Screen
	{
		readonly IEventAggregator events;

		[ImportingConstructor]
		public NotificationTestItem(IEventAggregator events) { this.events = events; }

		public void RaiseInfoMessage() { events.Publish(new NotificationRaised("Hi mom", NotificationLevel.Info)); }

		public void RaiseWarningMessage() { events.Publish(new NotificationRaised("Not a good idea... :-(", NotificationLevel.Warning)); }

		public void RaiseErrorMessage() { events.Publish(new NotificationRaised("Something really bad happened.", NotificationLevel.Error)); }

		public void RaiseLongMessage()
		{
			events.Publish(
				new NotificationRaised(
					@"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
					, NotificationLevel.Info));
		}
	}
}