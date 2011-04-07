namespace Raven.Studio.SamplePlugin.Tasks
{
	using System.ComponentModel.Composition;
	using Caliburn.Micro;
	using Messages;
	using Newtonsoft.Json.Linq;
	using Plugins;
	using Plugins.Tasks;

	[ExportTask("Pause Unit Test Marker")]
	public class SampleTask : Screen
	{
		readonly IServer server;
		readonly IEventAggregator events;
		const string DocumentId = "Sample/PauseUnitTest";
		string status;

		[ImportingConstructor]
		public SampleTask(IServer server, IEventAggregator events)
		{
			this.server = server;
			this.events = events;
			Status = "Ready to do something!";
		}

		public string Status
		{
			get { return status; }
			private set
			{
				status = value;
				NotifyOfPropertyChange(() => Status);
			}
		}

		protected override void OnActivate()
		{
			events.Publish(new WorkStarted("checking for marker document"));

			using (var session = server.OpenSession())
				session.Advanced.AsyncDatabaseCommands
					.GetAsync(DocumentId)
					.ContinueWith(_ =>
					{
						events.Publish(new WorkCompleted("checking for marker document"));

						CanDeleteMarkerDocument = (_.Result != null);
						CanAddMarkerDocument = (_.Result == null);

						Status = CanAddMarkerDocument
									? "No marker is present."
									: "Marker found.";
					});
		}

		bool canAddMarkerDocument;
		public bool CanAddMarkerDocument
		{
			get { return canAddMarkerDocument; }
			private set { canAddMarkerDocument = value; NotifyOfPropertyChange(() => CanAddMarkerDocument); }
		}

		bool canDeleteMarkerDocument;
		public bool CanDeleteMarkerDocument
		{
			get { return canDeleteMarkerDocument; }
			private set { canDeleteMarkerDocument = value; NotifyOfPropertyChange(() => CanDeleteMarkerDocument); }
		}

		public void AddMarkerDocument()
		{
			events.Publish(new WorkStarted("placing marker document"));
			CanDeleteMarkerDocument = false;
			CanAddMarkerDocument = false;

			using (var session = server.OpenSession())
				session.Advanced.AsyncDatabaseCommands
					.PutAsync(DocumentId, null, new JObject(), null)
					.ContinueWith(_ =>
					{
						events.Publish(new WorkCompleted("placing marker document"));
						Status = "Marker placed.";
						CanDeleteMarkerDocument = true;
						CanAddMarkerDocument = false;
					});
		}

		public void DeleteMarkerDocument()
		{
			events.Publish(new WorkStarted("deleting marker document"));
			CanDeleteMarkerDocument = false;
			CanAddMarkerDocument = false;

			using (var session = server.OpenSession())
				session.Advanced.AsyncDatabaseCommands
					.DeleteDocumentAsync(DocumentId)
					.ContinueWith(_ =>
									{
										events.Publish(new WorkCompleted("deleting marker document"));
										Status = "Marker Deleted!";
										CanDeleteMarkerDocument = false;
										CanAddMarkerDocument = true;
									});
		}
	}
}