using System ;
using System.ComponentModel ;
using System.Drawing ;
using System.IO ;
using System.Runtime.Serialization ;
using System.Runtime.Serialization.Formatters.Binary ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows.Forms.Design ;
using System.Windows.Input ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Command ;

namespace KayMcCormick.Lib.Wpf.Command
{
    /// <summary>
    /// </summary>
    public abstract class AppCommand : IDisplayableAppCommand , ICommand

    {
        /// <summary>
        /// </summary>
        /// <param name="displayName"></param>
        protected AppCommand ( string displayName ) { DisplayName = displayName ; }

        #region Implementation of IDisplayable
        /// <summary>
        /// </summary>
        public string DisplayName { get ; }
        #endregion

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// </summary>
        protected void OnCanExecuteChanged ( )
        {
            CanExecuteChanged?.Invoke ( this , EventArgs.Empty ) ;
        }

        #region Implementation of IAppCommand
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public abstract Task < IAppCommandResult > ExecuteAsync ( ) ;

        /// <summary>
        /// </summary>
        [ NotNull ] public ICommand Command { get { return this ; } }
        #endregion
        #region Implementation of ICommand
        /// <summary>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public abstract bool CanExecute ( object parameter ) ;

        /// <summary>
        /// </summary>
        /// <param name="parameter"></param>
        public virtual void Execute ( object parameter )
        {
            LastTask = ExecuteAsync ( )
               .ContinueWith (
                              ( task , state ) => {
                                  var appCommand = ( AppCommand ) state ;
                                  if ( task.IsFaulted )
                                  {
                                      // ReSharper disable once PossibleNullReferenceException
                                      DebugUtils.WriteLine ( task.Exception.ToString() ) ;
                                      appCommand.OnFault ( task.Exception ) ;
                                      return AppCommandResult.Faulted ( task.Exception ) ;
                                  }

                                  appCommand.OnResult ( task.Result ) ;
                                  return task.Result ;
                              }
                            , this
                            , CancellationToken.None
                            , TaskContinuationOptions.None
                            , TaskScheduler.FromCurrentSynchronizationContext ( )
                             ) ;
        }

        /// <summary>
        /// </summary>
        public Task < IAppCommandResult > LastTask { get ; set ; }

        /// <summary>
        /// </summary>
        /// <param name="exception"></param>
        public abstract void OnFault ( AggregateException exception ) ;

        /// <summary>
        /// </summary>
        /// <param name="result"></param>
        protected void OnResult ( IAppCommandResult result ) { LastResult = result ; }

        /// <summary>
        /// </summary>
        public IAppCommandResult LastResult { get ; set ; }

        /// <summary>
        /// </summary>
        public abstract object LargeImageSourceKey { get ; set ; }

        /// <summary>
        /// </summary>
        public abstract object Argument { get ; set ; }

        /// <summary>
        /// </summary>
        public event EventHandler CanExecuteChanged ;
        #endregion
    }


    /// <inheritdoc />
    [PropertyTabAttribute( typeof(TypeCategoryTab), PropertyTabScope.Document)]
    public sealed class TypeCategoryTabComponent : Component
    {
    }
    // A TypeCategoryTab property tab lists properties by the
    // category of the type of each property.
    /// <inheritdoc />
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public sealed class TypeCategoryTab : PropertyTab
    {
        [ Browsable ( true ) ]
// This string contains a Base-64 encoded and serialized example property tab image.
        private const string Img = "AAEAAAD/////AQAAAAAAAAAMAgAAAFRTeXN0ZW0uRHJhd2luZywgVmVyc2lvbj0xLjAuMzMwMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWIwM2Y1ZjdmMTFkNTBhM2EFAQAAABVTeXN0ZW0uRHJhd2luZy5CaXRtYXABAAAABERhdGEHAgIAAAAJAwAAAA8DAAAA9gAAAAJCTfYAAAAAAAAANgAAACgAAAAIAAAACAAAAAEAGAAAAAAAAAAAAMQOAADEDgAAAAAAAAAAAAD///////////////////////////////////9ZgABZgADzPz/zPz/zPz9AgP//////////gAD/gAD/AAD/AAD/AACKyub///////+AAACAAAAAAP8AAP8AAP9AgP////////9ZgABZgABz13hz13hz13hAgP//////////gAD/gACA/wCA/wCA/wAA//////////+AAACAAAAAAP8AAP8AAP9AgP////////////////////////////////////8L" ;

// Returns the properties of the specified component extended with
        // a CategoryAttribute reflecting the name of the type of the property.
        /// <inheritdoc />
        [ NotNull ]
        public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
        {
            var props = attributes == null ? TypeDescriptor.GetProperties(component) : TypeDescriptor.GetProperties(component, attributes) ;

            var propArray = new PropertyDescriptor[props.Count];
            for (var i = 0; i < props.Count; i++)
            {
                // Create a new PropertyDescriptor from the old one, with 
                // a CategoryAttribute matching the name of the type.
                propArray[i] = TypeDescriptor.CreateProperty(props[i].ComponentType, props[i], new CategoryAttribute(props[i].PropertyType.Name));
            }
            return new PropertyDescriptorCollection(propArray);
        }

        /// <inheritdoc />
        [ NotNull ]
        public override PropertyDescriptorCollection GetProperties(object component)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            return GetProperties(component, null);
        }

        // Provides the name for the property tab.
        /// <inheritdoc />
        [ NotNull ] public override string TabName
        {
            get
            {
                return "Properties by Type";
            }
        }

        // Provides an image for the property tab.
        /// <inheritdoc />
        [ NotNull ] public override Bitmap Bitmap
        {
            get
            {
                var bmp = new Bitmap(DeserializeFromBase64Text(Img));
                return bmp;
            }
        }

        // This method can be used to retrieve an Image from a block of Base64-encoded text.
        private Image DeserializeFromBase64Text([ NotNull ] string text)
        {
            Image deserializeFromBase64Text = null;
            var memBytes = Convert.FromBase64String(text);
            IFormatter formatter = new BinaryFormatter();
            var stream = new MemoryStream(memBytes);
            deserializeFromBase64Text = (Image)formatter.Deserialize(stream);
            stream.Close();
            return deserializeFromBase64Text;
        }
    }
}