using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace MafiatorApp.Behaviors
{
    public class EventToCommandBehavior : BehaviorBase<View>
	{
        private Delegate eventHandler;

		public static readonly BindableProperty EventNameProperty = BindableProperty.Create ("EventName", typeof(string), typeof(EventToCommandBehavior), propertyChanged: OnEventNameChanged);
		public static readonly BindableProperty CommandProperty = BindableProperty.Create ("Command", typeof(ICommand), typeof(EventToCommandBehavior));
		public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create ("CommandParameter", typeof(object), typeof(EventToCommandBehavior));
		public static readonly BindableProperty InputConverterProperty = BindableProperty.Create ("Converter", typeof(IValueConverter), typeof(EventToCommandBehavior));

		public string EventName {
			get => (string)GetValue (EventNameProperty);
            set => SetValue (EventNameProperty, value);
        }

		public ICommand Command {
			get => (ICommand)GetValue (CommandProperty);
            set => SetValue (CommandProperty, value);
        }

		public object CommandParameter {
			get => GetValue (CommandParameterProperty);
            set => SetValue (CommandParameterProperty, value);
        }

		public IValueConverter Converter {
			get => (IValueConverter)GetValue (InputConverterProperty);
            set => SetValue (InputConverterProperty, value);
        }

		protected override void OnAttachedTo (View bindable)
		{
			base.OnAttachedTo (bindable);
			RegisterEvent (EventName);
		}

		protected override void OnDetachingFrom (View bindable)
		{
            UnRegisterEvent(EventName);
			base.OnDetachingFrom (bindable);
		}

        private void RegisterEvent (string name)
		{
			if (string.IsNullOrWhiteSpace (name)) {
				return;
			}

			var eventInfo = AssociatedObject.GetType ().GetRuntimeEvent (name);
			if (eventInfo == null) {
				throw new ArgumentException ($"EventToCommandBehavior: Can't register the '{EventName}' event.");
			}
			var methodInfo = typeof(EventToCommandBehavior).GetTypeInfo ().GetDeclaredMethod ("OnEvent");
			eventHandler = methodInfo.CreateDelegate (eventInfo.EventHandlerType, this);
			eventInfo.AddEventHandler (AssociatedObject, eventHandler);
		}

        private void UnRegisterEvent (string name)
		{
			if (string.IsNullOrWhiteSpace (name)) {
				return;
			}

			if (eventHandler == null) {
				return;
			}
			var eventInfo = AssociatedObject.GetType ().GetRuntimeEvent (name);
			if (eventInfo == null) {
				throw new ArgumentException ($"EventToCommandBehavior: Can't de-register the '{EventName}' event.");
			}
			eventInfo.RemoveEventHandler (AssociatedObject, eventHandler);
			eventHandler = null;
		}

        private void OnEvent (object sender, object eventArgs)
		{
			if (Command == null) {
				return;
			}

			object resolvedParameter;
			if (CommandParameter != null) {
				resolvedParameter = CommandParameter;
			} else if (Converter != null) {
				resolvedParameter = Converter.Convert (eventArgs, typeof(object), null, null);
			} else {
				resolvedParameter = eventArgs;
			}		

			if (Command.CanExecute (resolvedParameter)) {
				Command.Execute (resolvedParameter);
			}
		}

        private static void OnEventNameChanged (BindableObject bindable, object oldValue, object newValue)
		{
			var behavior = (EventToCommandBehavior)bindable;
			if (behavior.AssociatedObject == null) {
				return;
			}

			var oldEventName = (string)oldValue;
			var newEventName = (string)newValue;

			behavior.UnRegisterEvent(oldEventName);
			behavior.RegisterEvent (newEventName);
		}
	}
}
