using System;

class BuildTypesView {
        [ TitleMetadata ( "Build Types View" ) ]
        [ UsedImplicitly ]
#pragma warning disable 1998
        public async Task BuildTypeViewAsync (
#pragma warning restore 1998
            [ NotNull ] IBaseLibCommand command
          , [ NotNull ] AppContext      context
        )
        {
            if ( command == null )
            {
                throw new ArgumentNullException ( nameof ( command ) ) ;
            }

            if ( context == null )
            {
                throw new ArgumentNullException ( nameof ( context ) ) ;
            }

            DebugUtils.WriteLine ( "Begin initialize TypeViewModel" ) ;

            using ( var db = new AppDbContext ( ) )
            {
                db.AppClrType.RemoveRange ( db.AppClrType ) ;
                db.AppTypeInfos.RemoveRange ( db.AppTypeInfos ) ;
                await db.SaveChangesAsync ( ) ;
            }

            using ( var db = new AppDbContext ( ) )
            {
                if ( db.AppTypeInfos.Any ( )
                     || db.AppClrType.Any ( ) )
                {
                    throw new InvalidOperationException ( ) ;
                }
            }

            var typesViewModel = TypesViewModel_Stage1 ( context ) ;

            var sts = context.Scope.Resolve < ISyntaxTypesService > ( ) ;
            var collectionMap = sts.CollectionMap ( ) ;

            SyntaxTypesService.LoadSyntax ( typesViewModel , collectionMap ) ;
            // foreach ( AppTypeInfo ati in typesViewModel.Map.Values )
            // {
            // typesViewModel.PopulateFieldTypes ( ati ) ;
            // }

            typesViewModel.DetailFields ( ) ;

            WriteModelToDatabase ( typesViewModel ) ;
            WriteThisTypesViewModel ( typesViewModel ) ;
            DumpModelToJson (
                             context
                           , typesViewModel
                           , Path.Combine ( DataOutputPath , TypesJsonFilename )
                            ) ;
        }

}
