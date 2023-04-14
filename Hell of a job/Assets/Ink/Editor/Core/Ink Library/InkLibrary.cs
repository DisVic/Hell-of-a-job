using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
<<<<<<< Updated upstream
=======
using System.Runtime.CompilerServices;
using NUnit.Framework;
>>>>>>> Stashed changes
using Debug = UnityEngine.Debug;

/// <summary>
/// Holds a reference to an InkFile object for every .ink file detected in the Assets folder.
/// Provides helper functions to easily obtain these files.
/// </summary>
namespace Ink.UnityIntegration {
    #if UNITY_2020_1_OR_NEWER
<<<<<<< Updated upstream
    [FilePath("Library/InkLibrary.asset", FilePathAttribute.Location.ProjectFolder)]
=======
    [FilePath("Library/asset", FilePathAttribute.Location.ProjectFolder)]
>>>>>>> Stashed changes
	public class InkLibrary : ScriptableSingleton<InkLibrary>, IEnumerable<InkFile> {
    #else
	public class InkLibrary : ScriptableObject, IEnumerable<InkFile> {
    #endif
<<<<<<< Updated upstream
        //
		public static System.Version inkVersionCurrent = new System.Version(1,0,0);
		public static System.Version unityIntegrationVersionCurrent = new System.Version(1,0,0);

		static string absoluteSavePath {
			get {
				return System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),"Library","InkLibrary.asset"));
=======
        // Ink version. This should really come from the core ink code.
		public static System.Version inkVersionCurrent = new System.Version(1,1,1);
		public static System.Version unityIntegrationVersionCurrent = new System.Version(1,1,7);

		static string absoluteSavePath {
			get {
				return System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),"Library","asset"));
>>>>>>> Stashed changes
			}
		}
		
		#if !UNITY_2020_1_OR_NEWER
		public static bool created {
			get {
				// If it's null, there's no InkLibrary loaded
				return (_instance != (Object) null);
			}
		}
		private static InkLibrary _instance;
		public static InkLibrary instance {
			get {
				if(!created)
                	LoadOrCreateInstance();
				return _instance;
			} private set {
				if(_instance == value) return;
				_instance = value;
            }
		}
        
		
		// This occurs on recompile, creation and load (note that data has not necessarily been loaded at this point!)
		protected InkLibrary () {
			if (created)
				Debug.LogError((object) "ScriptableSingleton already exists. Did you query the singleton in a constructor?");
			else {
				instance = this;
			}
		}

		public static void LoadOrCreateInstance () {
			InternalEditorUtility.LoadSerializedFileAndForget(absoluteSavePath);
			if(created) {
				if(InkEditorUtils.isFirstCompile) {
					Validate();
				}
			} else {
				instance = ScriptableObject.CreateInstance<InkLibrary>();
				instance.hideFlags = HideFlags.HideAndDontSave;
				Rebuild();
<<<<<<< Updated upstream
				instance.Save(true);
=======
>>>>>>> Stashed changes
			}
		}
		
		public void Save (bool saveAsText) {
			if(!created) return;			
			InternalEditorUtility.SaveToSerializedFileAndForget((Object[]) new InkLibrary[1] {instance}, absoluteSavePath, saveAsText);
		}

		static void EnsureCreated () {
			if(!created) LoadOrCreateInstance();
		}
        #endif
        
        public class AssetSaver : UnityEditor.AssetModificationProcessor {
            static string[] OnWillSaveAssets(string[] paths) {
                instance.Save(true);
                return paths;
            }
        }

		public List<InkFile> inkLibrary = new List<InkFile>();
<<<<<<< Updated upstream
		Dictionary<DefaultAsset, InkFile> inkLibraryDictionary;
=======
		Dictionary<DefaultAsset, InkFile> inkLibraryDictionary = new Dictionary<DefaultAsset, InkFile>();
>>>>>>> Stashed changes
		
        public int Count {
            get {
                return inkLibrary.Count;
            }
        }
        public InkFile this[int key] {
            get {
                return inkLibrary[key];
            } set {
                inkLibrary[key] = value;
            }
        }
        IEnumerator<InkFile> IEnumerable<InkFile>.GetEnumerator() {
            return inkLibrary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return inkLibrary.GetEnumerator();
        }

		void OnValidate () {
            BuildLookupDictionary();
<<<<<<< Updated upstream
            Validate();
=======
            // This is experimental! I'd like to see if it fixes the issue where assets have not yet been imported.
            EditorApplication.delayCall += () => {
                Validate();
            };
>>>>>>> Stashed changes
        }
		// After recompile, the data associated with the object is fetched (or whatever happens to it) by this point. 
		void OnEnable () {
			// Deletes the persistent version of this asset that we used to use prior to 0.9.71
			if(!Application.isPlaying && EditorUtility.IsPersistent(this)) {
				var path = AssetDatabase.GetAssetPath(this);
				if(!string.IsNullOrEmpty(path)) {
					#if !UNITY_2020_1_OR_NEWER
                    if(_instance == this) _instance = null;
					#endif
					AssetDatabase.DeleteAsset(path);
					AssetDatabase.Refresh();
					return;
				}
			}
		}

        static void BuildLookupDictionary () {
<<<<<<< Updated upstream
            if(instance.inkLibraryDictionary == null) instance.inkLibraryDictionary = new Dictionary<DefaultAsset, InkFile>();
            else instance.inkLibraryDictionary.Clear();
=======
            instance.inkLibraryDictionary.Clear();
>>>>>>> Stashed changes
			foreach(var inkFile in instance.inkLibrary) {
                instance.inkLibraryDictionary.Add(inkFile.inkAsset, inkFile);
            }
        }
        
		/// <summary>
		/// Checks if the library is corrupt and rebuilds if necessary. Returns true if the library was valid
		/// </summary>
        public static bool Validate () {
            if(RequiresRebuild()) {
                Rebuild();
                Debug.LogWarning("InkLibrary was invalid and has been rebuilt. This can occur if files are moved/deleted while the editor is closed. You can ignore this warning.");
				return false;
            } else {
				return true;
			}
        }
        
		/// <summary>
		/// Checks if the library is corrupt and requires a Rebuild. 
        /// This can happen when asset IDs change, causing the wrong file to be referenced.
        /// This occassionally occurs from source control.
        /// This is a fairly performant check.
		/// </summary>
        static bool RequiresRebuild () {
            #if !UNITY_2020_1_OR_NEWER
			EnsureCreated();
            #endif
			foreach(var inkFile in instance.inkLibrary) {
                if(inkFile == null) {
                    return true;
                }
                if(inkFile.inkAsset == null) {
<<<<<<< Updated upstream
=======
                    // This can occur when the asset has not yet been imported!
>>>>>>> Stashed changes
                    return true;
                }
                if(!instance.inkLibraryDictionary.ContainsKey(inkFile.inkAsset)) {
                    return true;
                }
<<<<<<< Updated upstream
                if(inkFile.inkAsset == null) {
                    return true;
                }
=======
>>>>>>> Stashed changes
                foreach(var include in inkFile.includes) {
                    if(include == null) {
                        return true;
                    }
                    if(!instance.inkLibraryDictionary.ContainsKey(include)) {
                        return true;
                    }
                } 
            }
            return false;
        }

		/// <summary>
		/// Removes and null references in the library
		/// </summary>
		public static bool Clean () {
            bool wasDirty = false;
			for (int i = instance.Count - 1; i >= 0; i--) {
<<<<<<< Updated upstream
				InkFile inkFile = InkLibrary.instance[i];
				if (inkFile.inkAsset == null) {
					InkLibrary.RemoveAt(i);
=======
				InkFile inkFile = instance[i];
				if (inkFile.inkAsset == null) {
					RemoveAt(i);
>>>>>>> Stashed changes
                    wasDirty = true;
                }
			}
            return wasDirty;
		}

<<<<<<< Updated upstream
        public static void Add (InkFile inkFile) {
=======
        static void Add (InkFile inkFile) {
>>>>>>> Stashed changes
            instance.inkLibrary.Add(inkFile);
			SortInkLibrary();
			instance.inkLibraryDictionary.Add(inkFile.inkAsset, inkFile);
        }
        public static void RemoveAt (int index) {
            var inkFile = instance.inkLibrary[index];
            instance.inkLibrary.RemoveAt(index);
            instance.inkLibraryDictionary.Remove(inkFile.inkAsset);
        }
		static void SortInkLibrary () {
            instance.inkLibrary = instance.inkLibrary.OrderBy(x => x.filePath).ToList();
		}

		/// <summary>
		/// Updates the ink library. Executed whenever an ink file is changed by InkToJSONPostProcessor
		/// Can be called manually, but incurs a performance cost.
		/// </summary>
		public static void Rebuild () {
			// Disable the asset post processor in case any assetdatabase functions called as a result of this would cause further operations.
			InkPostProcessor.disabled = true;
			
<<<<<<< Updated upstream
            // Remove any old file connections
            Clean();
=======
            // Clear the old data
            instance.inkLibrary.Clear();
            instance.inkLibraryDictionary.Clear();
>>>>>>> Stashed changes

			// Reset the asset name
			instance.name = "Ink Library "+unityIntegrationVersionCurrent.ToString();
            
			// Add any new file connections (if any are found it replaces the old library entirely)
			string[] inkFilePaths = GetAllInkFilePaths();
			bool inkLibraryChanged = false;
			List<InkFile> newInkLibrary = new List<InkFile>(inkFilePaths.Length);
			for (int i = 0; i < inkFilePaths.Length; i++) {
				InkFile inkFile = GetInkFileWithAbsolutePath(inkFilePaths [i]);
				// If the ink library doesn't have a representation for this file, then make one 
                if(inkFile == null) {
					inkLibraryChanged = true;
					string localAssetPath = InkEditorUtils.AbsoluteToUnityRelativePath(inkFilePaths [i]);
					DefaultAsset inkFileAsset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(localAssetPath);
					// If the ink file can't be found, it might not yet have been imported. We try to manually import it to fix this.
					if(inkFileAsset == null) {
						AssetDatabase.ImportAsset(localAssetPath);
						inkFileAsset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(localAssetPath);
						if(inkFileAsset == null) {
<<<<<<< Updated upstream
=======
                            // If this occurs as a result assets not having been imported before OnValidate => Validate we should return immediately and set a flag to true.
                            // If an asset import is detected immediately after this via InkPostProcessor, then this rebuild may (will?) have been unnecessary anyway.
                            // At time of writing (11/05/21) I've not done this and am locally toying with EditorApplication.delayCall in OnValidate.
>>>>>>> Stashed changes
							Debug.LogWarning("Ink File Asset not found at "+localAssetPath+". This can occur if the .meta file has not yet been created. This issue should resolve itself, but if unexpected errors occur, rebuild Ink Library using Assets > Recompile Ink");
							continue;
						}
					}
					inkFile = new InkFile(inkFileAsset);
				}
<<<<<<< Updated upstream
				newInkLibrary.Add(inkFile);
=======
                newInkLibrary.Add(inkFile);
>>>>>>> Stashed changes
			}
			if(inkLibraryChanged) {
				instance.inkLibrary = newInkLibrary;
				SortInkLibrary();
			}
            BuildLookupDictionary();

			RebuildInkFileConnections();

			foreach (InkFile inkFile in instance.inkLibrary) inkFile.FindCompiledJSONAsset();
<<<<<<< Updated upstream
=======

			// if(InkSettings.instance.handleJSONFilesAutomatically) DeleteUnwantedCompiledJSONAssets();
			
>>>>>>> Stashed changes
			instance.Save(true);
			
			// Re-enable the ink asset post processor
			InkPostProcessor.disabled = false;
<<<<<<< Updated upstream
			Debug.Log("Ink Library was rebuilt.");
		}

		public static void CreateOrReadUpdatedInkFiles (List<string> importedInkAssets) {
			foreach (var importedAssetPath in importedInkAssets) {
				InkFile inkFile = InkLibrary.GetInkFileWithPath(importedAssetPath);
				if(inkFile == null) {
					DefaultAsset asset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(importedAssetPath);
					inkFile = new InkFile(asset);
					Add(inkFile);
				} else {
=======
			Debug.Log("Ink Library was rebuilt.\n"+instance.inkLibrary.Count+" ink files are currently tracked.");
		}

		// To be used when adding .ink files. 
		// This process is typically handled by CreateOrReadUpdatedInkFiles, called from InkPostProcessor; but it may be desired to remove/disable the post processor.
		// In those cases, this is the correct way to ensure the ink library correctly processes the file.
		public static InkFile AddNewInkFile (DefaultAsset asset) {
			Debug.Assert(asset != null);
			// First, check if we've already got it in the library!
			foreach(var _inkFile in instance)
				if(_inkFile.inkAsset == asset)
					return _inkFile;
			// If not
			var inkFile = new InkFile(asset);
			inkFile.FindCompiledJSONAsset();
			Add(inkFile);
			RebuildInkFileConnections();
			return inkFile;
		}

		// This is called from InkPostProcessor after ink file(s) has been added/changed.
		public static void CreateOrReadUpdatedInkFiles (List<string> importedInkAssets) {
            for (int i = 0; i < importedInkAssets.Count; i++) {
                string importedAssetPath = importedInkAssets[i];
                InkFile inkFile = GetInkFileWithPath(importedAssetPath);
				if(inkFile == null) {
					DefaultAsset asset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(importedAssetPath);
					if(asset == null) {
						// This file wasn't found! This is a rare bug. We remove the file from the list in this case, preventing it from causing further bugs.
						importedInkAssets.RemoveAt(i);
						i--;
						Debug.LogWarning("InkLibrary failed to load ink file at "+importedAssetPath+". It has been removed from the list of files. You can ignore this warning.");
					} else {
						// New file; create and add InkFile to represent it. Content is read in InkFile constructor.
						inkFile = new InkFile(asset);
						inkFile.FindCompiledJSONAsset();
						Add(inkFile);
					}
				} else {
					// Read content
>>>>>>> Stashed changes
					inkFile.ParseContent();
				}
			}
			// Now we've updated all the include paths for the ink library we can create master/child references between them.
			RebuildInkFileConnections();
		}

        // Finds absolute file paths of all the ink files in Application.dataPath
		private static string[] GetAllInkFilePaths () {
			string[] inkFilePaths = Directory.GetFiles(Application.dataPath, "*.ink", SearchOption.AllDirectories);
			for (int i = 0; i < inkFilePaths.Length; i++) {
				inkFilePaths [i] = InkEditorUtils.SanitizePathString(inkFilePaths [i]);
			}
			return inkFilePaths;
		}

		// All the master files
		public static IEnumerable<InkFile> GetMasterInkFiles () {
			if(instance.inkLibrary == null) yield break;
			foreach (InkFile inkFile in instance.inkLibrary) {
				if(inkFile.isMaster) 
					yield return inkFile;
			}
		}

<<<<<<< Updated upstream
		// All the master files which are dirty and are set to compile
		public static IEnumerable<InkFile> GetFilesRequiringRecompile () {
			foreach(InkFile inkFile in InkLibrary.GetMasterInkFiles ()) {
				if(inkFile.requiresCompile && (InkSettings.instance.compileAutomatically || inkFile.compileAutomatically)) 
=======
		public static IEnumerable<InkFile> GetInkFilesMarkedToCompileAsMasterFiles () {
			if(instance.inkLibrary == null) yield break;
			foreach (InkFile inkFile in instance.inkLibrary) {
				if(inkFile.isMaster) 
					yield return inkFile;
			}
		}

		// All the master files which are dirty and are set to compile
		public static IEnumerable<InkFile> GetFilesRequiringRecompile () {
			foreach(InkFile inkFile in GetInkFilesMarkedToCompileAsMasterFiles ()) {
				if(InkSettings.instance.ShouldCompileInkFileAutomatically(inkFile) && inkFile.requiresCompile) 
>>>>>>> Stashed changes
					yield return inkFile;
			}
		}

		// All the master files which are set to compile
		public static IEnumerable<InkFile> FilesCompiledByRecompileAll () {
<<<<<<< Updated upstream
			foreach(InkFile inkFile in InkLibrary.GetMasterInkFiles ()) {
				if(InkSettings.instance.compileAutomatically || inkFile.compileAutomatically) 
=======
			foreach(InkFile inkFile in GetInkFilesMarkedToCompileAsMasterFiles ()) {
				if(InkSettings.instance.ShouldCompileInkFileAutomatically(inkFile)) 
>>>>>>> Stashed changes
					yield return inkFile;
			}
		}

		/// <summary>
		/// Gets the ink file from the .ink file reference.
		/// </summary>
		/// <returns>The ink file with path.</returns>
		/// <param name="file">File asset.</param>
		/// <param name="addIfMissing">Adds the file if missing from inkLibrary.</param>
		public static InkFile GetInkFileWithFile (DefaultAsset file, bool addIfMissing = false) {
			if(instance.inkLibrary == null) return null;
			
			if (!file) {
				Debug.LogError("Can't add null file.");
				return null;
			}

            if(instance.inkLibraryDictionary == null) {
				Debug.LogWarning("GetInkFileWithFile: inkLibraryDictionary was null! This should never occur, but is handled following a user reported bug. If this has never been seen long after 12/08/2020, it can be safely removed");
				BuildLookupDictionary();
			}
			foreach(InkFile inkFile in instance.inkLibrary) {
				if(inkFile.inkAsset == file) {
					return inkFile;
				}
			}

<<<<<<< Updated upstream
			if (addIfMissing) {
				InkFile newFile = new InkFile(file);
				instance.inkLibrary.Add(newFile);
				Debug.Log(file + " missing from ink library. Adding it now.");
=======
			var missingFileHasProperFileExtension = Path.GetExtension(AssetDatabase.GetAssetPath(file)) == InkEditorUtils.inkFileExtension;
			if (addIfMissing) {
				InkFile newFile = new InkFile(file);
				instance.inkLibrary.Add(newFile);
				if(missingFileHasProperFileExtension) Debug.Log(file + " missing from ink library. Adding it now.");
				else Debug.LogWarning("File "+file + " is missing the .ink extension, but is believed to be an ink file. All ink files should use the .ink file extension! A common effect of this is forcing the InkLibrary to rebuild unexpectedly when the file is detected as an include of another file.");
>>>>>>> Stashed changes
				return newFile;
			}

			System.Text.StringBuilder listOfFiles = new System.Text.StringBuilder();
			foreach(InkFile inkFile in instance.inkLibrary) {
				listOfFiles.AppendLine(inkFile.ToString());
			}
<<<<<<< Updated upstream
			Debug.LogWarning (file + " missing from ink library. Please rebuild.\n"+listOfFiles);
=======
			if(missingFileHasProperFileExtension) Debug.LogWarning (file + " missing from ink library. Please rebuild.\nFiles in Library:\n"+listOfFiles);
			else Debug.LogWarning (file + " is missing from ink library. It is also missing the .ink file extension. All ink files should use the .ink file extension! \nFiles in Library:\n"+listOfFiles);
>>>>>>> Stashed changes
			return null;
		}

		/// <summary>
		/// Gets the ink file with path relative to Assets folder, for example: "Assets/Ink/myStory.ink".
		/// </summary>
		/// <returns>The ink file with path.</returns>
		/// <param name="path">Path.</param>
		public static InkFile GetInkFileWithPath (string path) {
			if(instance.inkLibrary == null) return null;
			foreach(InkFile inkFile in instance.inkLibrary) {
				if(inkFile.filePath == path) {
					return inkFile;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the ink file with absolute path.
		/// </summary>
		/// <returns>The ink file with path.</returns>
		/// <param name="path">Path.</param>
		public static InkFile GetInkFileWithAbsolutePath (string absolutePath) {
			if(instance.inkLibrary == null) return null;
			foreach(InkFile inkFile in instance.inkLibrary) {
				if(inkFile.absoluteFilePath == absolutePath) {
					return inkFile;
				}
			}
			return null;
		}

<<<<<<< Updated upstream

		/// <summary>
		/// Rebuilds which files are master files and the connections between the files.
		/// </summary>
		public static void RebuildInkFileConnections () {
			Queue<InkFile> inkFileQueue = new Queue<InkFile>(instance.inkLibrary);
			while (inkFileQueue.Count > 0) {
				InkFile inkFile = inkFileQueue.Dequeue();
				inkFile.parents = new List<DefaultAsset>();
				inkFile.masterInkAssets = new List<DefaultAsset>();
				inkFile.ParseContent();
				inkFile.FindIncludedFiles(true);

				foreach (InkFile includedInkFile in inkFile.includesInkFiles) {
					if (!inkFileQueue.Contains(includedInkFile)) {
						inkFileQueue.Enqueue(includedInkFile);
					}
				}
			}

			// We now set the master file for ink files. As a file can be in an include hierarchy, we need to do this in two passes.
			// First, we set the master file to the file that includes an ink file.
			foreach (InkFile inkFile in instance.inkLibrary) {
				if(inkFile.includes.Count == 0) 
					continue;
				foreach (InkFile otherInkFile in instance.inkLibrary) {
					if(inkFile == otherInkFile) 
						continue;
					if(inkFile.includes.Contains(otherInkFile.inkAsset)) {
						if(!otherInkFile.parents.Contains(inkFile.inkAsset)) {
							otherInkFile.parents.Add(inkFile.inkAsset);
						}
					}
				}
			}
			// Next, we create a list of all the files owned by the actual master file, which we obtain by travelling up the parent tree from each file.
			Dictionary<InkFile, List<InkFile>> masterChildRelationships = new Dictionary<InkFile, List<InkFile>>();
			foreach (InkFile inkFile in instance.inkLibrary) {
				foreach(var parentInkFile in inkFile.parentInkFiles) {
					InkFile lastMasterInkFile = parentInkFile;
					InkFile masterInkFile = parentInkFile;
					while (masterInkFile.parents.Count != 0) {
						// This shouldn't just pick first, but iterate the whole lot! 
						// I didn't feel like writing a recursive algorithm until it's actually needed though - a file included by several parents is already a rare enough case!
						masterInkFile = masterInkFile.parentInkFiles.First();
						lastMasterInkFile = masterInkFile;
					}
					if(lastMasterInkFile.parents.Count > 1) {
						Debug.LogError("The ink ownership tree has another master file that is not discovered! This is an oversight of the current implementation. If you requres this feature, please take a look at the comment in the code above - if you solve it let us know and we'll merge it in!");
					}
					if(!masterChildRelationships.ContainsKey(masterInkFile)) {
						masterChildRelationships.Add(masterInkFile, new List<InkFile>());
					}
					masterChildRelationships[masterInkFile].Add(inkFile);
				}

				// if(inkFile.parent == null) 
				// 	continue;
				// InkFile parent = inkFile.parentInkFile;
				// while (parent.metaInfo.parent != null) {
				// 	parent = parent.metaInfo.parentInkFile;
				// }
				// if(!masterChildRelationships.ContainsKey(parent)) {
				// 	masterChildRelationships.Add(parent, new List<InkFile>());
				// }
				// masterChildRelationships[parent].Add(inkFile);
			}
			// Finally, we set the master file of the children
			foreach (var inkFileRelationship in masterChildRelationships) {
				foreach(InkFile childInkFile in inkFileRelationship.Value) {
					if(!childInkFile.masterInkAssets.Contains(inkFileRelationship.Key.inkAsset)) {
						childInkFile.masterInkAssets.Add(inkFileRelationship.Key.inkAsset);
					} else {
						Debug.LogWarning("Child file already contained master file reference! This is weird!");
					}
					if(InkSettings.instance.handleJSONFilesAutomatically && childInkFile.jsonAsset != null) {
						AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(childInkFile.jsonAsset));
						childInkFile.jsonAsset = null;
					}
=======
		public static InkFile GetInkFileWithJSONFile (TextAsset inkJSONAsset) {
			if(instance.inkLibrary == null) return null;
			foreach(InkFile inkFile in instance.inkLibrary) {
				if(inkFile.jsonAsset == inkJSONAsset) {
					return inkFile;
				}
			}
			return null;
		}

		/// <summary>
		/// Rebuilds which files are master files and the connections between the files.
		/// INCLUDE is always relative to the master file. This means that every file should be assumed to be a master file until proven otherwise.
		/// </summary>
		/// We might consider updating this to allow rebuilding connections for specific files, since the most common case this is called is when a single file changes.
		/// The upside is that we wouldn't trigger warnings/errors that this function throws, for unrelated files. It's a bit risky so I've not done it yet.
		public static void RebuildInkFileConnections () {
			// Resets the connections between files
			foreach (InkFile inkFile in instance.inkLibrary) {
				inkFile.recursiveIncludeErrorPaths.Clear();
				inkFile.ClearAllHierarchyConnections();
			}
			
			
			// A dictionary which contains a list of all the ink files that INCLUDE a given ink file.
			// Once this is done we can determine which files are master files, and then assert that any INCLUDED files actually exist.
			Dictionary<InkFile, List<InkFile>> includedFileOwnerDictionary = new Dictionary<InkFile, List<InkFile>>();
			Dictionary<InkFile, List<InkFile>> recursiveIncludeLogs = new Dictionary<InkFile, List<InkFile>>();
			// Traverses each file to any file paths referenced using INCLUDE, using the original file as the source path when dealing with nested INCLUDES. 
			// Since not all of the files are guaranteed to be master files, we don't assert that the files actually exist at this time.
			foreach (InkFile inkFile in instance.inkLibrary) {
				BuildIncludeHierarchyAsIfMasterFile(inkFile, inkFile, recursiveIncludeLogs);
				// Recurse ink file includes for a (potential) master ink file, adding them to the file's list of includes if they exist
				static void BuildIncludeHierarchyAsIfMasterFile(InkFile potentialMasterInkFile, InkFile currentInkFile, Dictionary<InkFile, List<InkFile>> recursiveIncludeLogs) {
					if(currentInkFile.localIncludePaths.Count == 0) 
						return;
					foreach (var includePath in currentInkFile.localIncludePaths) {
						var includedFile = FindIncludedFile(potentialMasterInkFile.filePath, includePath);
						// Assets may not actually exist.
						// A typical and expected example is when an included file in a subfolder from it's master file has an INCLUDE, since file paths are always relative to the master file. 
						if (includedFile != null) {
							// We probably only need to show this error for files that are later proved to be master files
							if (potentialMasterInkFile == includedFile || potentialMasterInkFile.includes.Contains(includedFile.inkAsset)) {
								if(!recursiveIncludeLogs.ContainsKey(potentialMasterInkFile)) recursiveIncludeLogs.Add(potentialMasterInkFile, new List<InkFile>());
								recursiveIncludeLogs[potentialMasterInkFile].Add(currentInkFile); 
								continue;
							}
							Debug.Assert(includedFile.inkAsset != null);
							potentialMasterInkFile.includes.Add(includedFile.inkAsset);
							BuildIncludeHierarchyAsIfMasterFile(potentialMasterInkFile, includedFile, recursiveIncludeLogs);
						}
					}
					
					static InkFile FindIncludedFile(string masterFilePath, string includePath) {
						string localIncludePath = InkEditorUtils.CombinePaths(Path.GetDirectoryName(masterFilePath), includePath);
						// This enables parsing ..\ and the like. Can we use Path.GetFullPath instead?
						var fullIncludePath = new FileInfo(localIncludePath).FullName;
						localIncludePath = InkEditorUtils.AbsoluteToUnityRelativePath(fullIncludePath);
						DefaultAsset includedInkFileAsset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(localIncludePath);
						if(includedInkFileAsset != null) {
							return GetInkFileWithFile(includedInkFileAsset);
						}
						return null;
					}
				}
				
				foreach (var includedFile in inkFile.includes) {
					var includedInkFile = GetInkFileWithFile(includedFile);
					if(!includedFileOwnerDictionary.ContainsKey(includedInkFile)) includedFileOwnerDictionary.Add(includedInkFile, new List<InkFile>());
					includedFileOwnerDictionary[includedInkFile].Add(inkFile);
				}
			}
			
			// Now we've established which files are INCLUDED we can tidy up by detecting and removing non-master files.
			// It's not a master file then we remove all references to it as a master file in the includedFileOwnerDictionary.
			// We don't clear the includes list for those files (even though referenced files may be null), because the user may mark isMarkedToCompileAsMasterFile true at a later date.
			foreach (InkFile inkFile in instance.inkLibrary) {
				var isMasterFile = !includedFileOwnerDictionary.ContainsKey(inkFile) || includedFileOwnerDictionary[inkFile].Count == 0 || inkFile.isMarkedToCompileAsMasterFile;
				if (!isMasterFile) {
					foreach (var includedFileOwners in includedFileOwnerDictionary) {
						if(includedFileOwners.Key != inkFile) includedFileOwners.Value.Remove(inkFile);
					}
				}
			}
			
			// Master ink files and includedFileOwnerDictionary are now valid collections denoting master files and their includes. The final step is to add the masters in any included files.
			foreach (InkFile inkFile in instance.inkLibrary) {
				if (!includedFileOwnerDictionary.ContainsKey(inkFile) || includedFileOwnerDictionary[inkFile].Count == 0 || inkFile.isMarkedToCompileAsMasterFile) {
					foreach (var includedFile in inkFile.includes) {
						var includedInkFile = GetInkFileWithFile(includedFile);
						includedInkFile.masterInkAssets.Add(inkFile.inkAsset);
					}

				}
			}
			
			// Error logs for any master files that wanted to add recursive includes
			foreach (var recursiveIncludeLog in recursiveIncludeLogs) {
				if (recursiveIncludeLog.Key.isMaster) {
					recursiveIncludeLog.Key.recursiveIncludeErrorPaths.AddRange(recursiveIncludeLog.Value.Select(x => x.filePath));
					var files = string.Join("\n", recursiveIncludeLog.Key.recursiveIncludeErrorPaths);
					Debug.LogError("Recursive INCLUDE found in "+recursiveIncludeLog.Key.filePath+" at "+(recursiveIncludeLog.Value.Count == 1 ? "file:\n" : "files:\n")+files);
				}
			}
		}
		
		// Deletes any JSON ink assets that aren't expected to exist because their ink files aren't expected to be compiled
		public static void DeleteUnwantedCompiledJSONAssets() {
			foreach (InkFile inkFile in instance.inkLibrary) {
				if(!inkFile.isMaster && inkFile.jsonAsset != null) {
					AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(inkFile.jsonAsset));
					inkFile.jsonAsset = null;
>>>>>>> Stashed changes
				}
			}
		}
	}
}