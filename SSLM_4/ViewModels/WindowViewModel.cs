﻿using System;
using System.Windows;
using System.Windows.Input;
using System.Runtime.InteropServices;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HNTR.Models;

namespace HNTR.ViewModels {
	public class WindowViewModel : ViewModelBase {
		#region Variables
		private Window _Window;
		private int _OuterMarginSize = 10;
		private int _WindowRadius = 10;
		#endregion

		#region Commands
		public ICommand MinimizeCommand { get; set; }
		public ICommand MaximizeCommand { get; set; }
		public ICommand CloseCommand { get; set; }
		public ICommand MenuCommand { get; set; }
		#endregion

		#region Properties
		public string Title { get; private set; }

		public int BorderThickness { get; set; } = 6;
		public int TitleHeight { get; set; } = 36;
		public double MinWindowWidth { get; set; } = 640;
		public double MinWindowHeight { get; set; } = 480;

		public GridLength TitleHeightGridLength { get { return new GridLength(TitleHeight + BorderThickness); } }
		public Thickness ResizeBorderThickness { get { return new Thickness(BorderThickness + OuterMarginSize); } }
		public Thickness OuterMarginSizeThickness { get { return new Thickness(OuterMarginSize); } }
		public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }

		public int OuterMarginSize {
			get {
				return _Window.WindowState == WindowState.Maximized ? 0 : _OuterMarginSize;
			}

			set {
				_OuterMarginSize = value;
			}
		}

		public int WindowRadius {
			get {
				return _Window.WindowState == WindowState.Maximized ? 0 : _WindowRadius;
			}

			set {
				_WindowRadius = value;
			}
		}
		#endregion

		public WindowViewModel(Window window) {
			_Window = window;
			Title = Constants.MAIN_TITLE;

			_Window.StateChanged += (sedner, e) => {
				RaisePropertyChanged(nameof(ResizeBorderThickness));
				RaisePropertyChanged(nameof(OuterMarginSizeThickness));
				RaisePropertyChanged(nameof(WindowCornerRadius));
				RaisePropertyChanged(nameof(OuterMarginSize));
				RaisePropertyChanged(nameof(WindowRadius));
			};

			MinimizeCommand = new RelayCommand(() => _Window.WindowState = WindowState.Minimized);
			MaximizeCommand = new RelayCommand(() => _Window.WindowState ^= WindowState.Maximized);
			CloseCommand = new RelayCommand(() => _Window.Close());
			MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(_Window, GetMousePosition()));
		}

		#region Helpers
		private Point GetMousePosition()
		{
			var position = Mouse.GetPosition(_Window);
			return new Point(position.X + _Window.Left, position.Y + _Window.Top);
		}
		#endregion
	}
}
