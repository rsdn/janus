using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Сервис управления окном браузера.
	/// </summary>
	public interface IBrowserFormService
	{
		void NavigateTo(string url);
		void NavigateBackward();
		void NavigateForward();
		void Refresh();
		void Stop();
		void Close();

		string Url { get; }
		bool CanNavigateBackward { get; }
		bool CanNavigateForward { get; }
		bool CanStop { get; }

		event EventHandler Navigated;
		event EventHandler CanNavigateBackwardChanged;
		event EventHandler CanNavigateForwardChanged;
		event EventHandler DocumentCompleted;
	}
}