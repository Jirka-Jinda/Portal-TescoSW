namespace Models
{
	public class DispatcherControlModel
	{
		public string Dispatcher { get; set; }
		public AppState State { get; set; }

		public DispatcherControlModel() => (Dispatcher, State) = ("", AppState.Unknown);

		public DispatcherControlModel(AppLinksModel app)
		{
			Dispatcher = app.Name + "Dispatcher";
			State = AppState.Unknown;
			//get correct dispatcher info and create object
		}
	}
}
