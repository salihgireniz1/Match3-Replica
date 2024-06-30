using Zenject;
using System.Collections.Generic;

namespace Match3
{
    /// <summary>
    /// A class that provides a list of SelectionProcess instances and allows for retrieving a valid SelectionProcess for a given pair of GridObjects.
    /// </summary>
    public class SelectionProvider
    {
        /// <summary>
        /// Gets the list of SelectionProcess instances that have been registered with Zenject.
        /// </summary>
        /// <value>
        /// A list of SelectionProcess instances.
        /// </value>
        public List<Selection> SelectionProcesses => selectionProcesses;

        List<Selection> selectionProcesses = new();


        /// <summary>
        /// Automatically gets every instance of SelectionProcess class that has been
        /// registered with Zenject. 

        /// Check ResolveAll() functionality of Zenject for more information. 

        /// The [Inject] at the beginning kinda works as ResolveAll() but automatically.
        /// </summary>
        /// <param name="selectionProcesses">The list of SelectionProcess instances 
        /// that have been registered with Zenject.
        /// </param>
        public SelectionProvider([Inject] List<Selection> selectionProcesses)
        {
            this.selectionProcesses = selectionProcesses;
        }

        /// <summary>
        /// Retrieves a valid SelectionProcess for the given pair of GridObjects.
        /// </summary>
        /// <param name="gridObject1">The first GridObject.</param>
        /// <param name="gridObject2">The second GridObject.</param>
        /// <returns>A valid SelectionProcess for the given pair of GridObjects, or null if no valid SelectionProcess is found.</returns>
        public Selection GetValidSelectionProcess(GridObject<BaseGem> gridObject1, GridObject<BaseGem> gridObject2)
        {
            foreach (var process in selectionProcesses)
            {
                if (process.IsValid(gridObject1?.GetValue(), gridObject2?.GetValue()))
                {
                    return process;
                }
            }
            return null;
        }
    }

}