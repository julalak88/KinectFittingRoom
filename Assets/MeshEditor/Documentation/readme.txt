Mesh Editor - Advanced Mesh Editing Utility
v1.9 - Released 10/11/2017 by Alan Baylis
----------------------------------------


Foreword
----------------------------------------
Thank you for your purchase of Mesh Editor. The program has recently had a complete rebuild. The feature list has grown very long and contains all of the functions you would expect in an advanced mesh editing utility. If you have a feature request then please let us know by writing to support@meshmaker.com.

Features:
	- Unity 5 Ready
	- Works with Skinned Meshes
	- Subdivide Selected Triangles
	- Lock/Unlock Target Object
	- Save Mesh
	- Save Prefab
	- Custom Save Paths
	- Select All
	- Clear Selected
	- Shift To Deselect
	- Invert Selected
	- Delete Triangles
	- Double Sided Triangles
	- Reverse Triangles
	- Smooth Triangles
	- Facet Triangles
	- Normalize Scale
	- Vertex Move/Rotate/Scale
	- Edge Move/Rotate/Scale
	- Triangle Move/Rotate/Scale
	- Texture Move/Rotate/Scale
	- Extrude Edges
	- Extrude Triangles
	- Auto UV Update
	- Local/World Orientation
	- Front Selection Toggle
	- Render Wireframe
	- Quick Highlights
	- Snap To Grid
	- Snap To Rotation
	- Optimize Mesh
	- Flip X, Y, Z Axes
	- Revert Mesh To Original
	- Set/Adjust Pivot Point
	- Auto Save Settings
	- Manual Save/Load Settings
	- Load Default Settings
	- Full Undo/Redo
	- Customizable Colors
	- Selectable Hotkey
	- Adjustable Epsilon Value
	- Max Smoothing Angle
	- Full Marquee Selection
	- Split & Cut Meshes
	- Create Submeshes
	- Export to OBJ Format
	- Tooltips
	- Keyboard Hotkeys
	- Split Edges
	- Fill Holes
	- Weld Vertices/Edges
	- Un-Weld Vertices/Edges
	- Join Vertices/Edges
	- Triangle/Hole Filling
	- Individual UV Editing
	- Nine Custom Primitives
	- Modify Normals
	- Check Invalid Mesh
	
	
Notes
----------------------------------------
You can access the other pages by clicking on the toolbar at the top of the Mesh Editor window, where it says Edit.

If you are experiencing problems selecting vertices/triangles you can now change the epsilon value in the settings.

When extruding multiple triangles that meet at a single point they sometimes have inverted faces. The solution is to extrude the triangles individually.

In the last update the keys to extrude have been changed from the shift key to the ctrl+shift keys to avoid deselecting triangles and edges when extruding.

All features work with skinned meshes except for extrusion.

Will preserve the Blend Shapes except when cutting or deleting.


ToDo List.
----------------------------------------
Done - Extrusion and deformation of meshes
Done - Skinned mesh support
Done - Bridge edges
Done - Combine/Split vertices
Done - Vertex snap
Done - Full mesh optimization
Done - Edge Splitting
Done - Modify Normals
- Edge loops


Common Issues / FAQ
----------------------------------------
Please visit the home page at http://www.meshmaker.com for the latest news and help forum.


Contact
----------------------------------------
Alan Baylis
www.meshmaker.com
support@meshmaker.com


Attributions
----------------------------------------
The scene used in the Mesh Editor v1.4 update video was kindly provided for free by Patryk Zatylny here on the UAS at https://www.assetstore.unity3d.com/en/#!/content/35361


Update Log
-----------------------------------------
v0.7.0 released 02/08/14
Beta release of Mesh Editor.

v0.7.1 released 06/08/14
Fixed bug when deleting multiple triangles
Fixed undo/redo step names
Fixed double undo for delete
Cleaned up more memory leaks
Added option to change epsilon values

v0.7.2 released 03/09/14
New GUI layout
Fixes here and there
 
v0.7.4 released 31/03/15
Updated for Unity 4.6.2 and 5.0

v1.0 released 15/06/15
Fully rebuilt from the ground up
New compact GUI
Added Hotkeys
Edge extrusion
Full undo/redo
Reverse selected triangles
Double side selected triangles
Smooth/Facet seletcted triangles
& much more!

v1.1 released 31/07/15
Changed the mesh copy dialog to select both the filename and location

v1.2 released 14/08/15
Skinned mesh support added
Subdivision of triangles added

v1.3 released 04/10/15
New option to show normals
New option to recalculate normals
New option to recalculate tangents
Update UVs now works with normal editing
Locked object stays locked after delete, etc
Fix to Snap To Grid
Fix to shader warning
Fix to handle position after extrude
Fix to double sided normals
Added tool to generate secondary UV set
Added tool to recalculate normals

v1.4 released 29/04/16
Added Edge Splitting
Added Triangle/Hole Filling
Added Individual UV Editing
Added Procedural Geometry - Basic 2D/3D Shapes
Added Modify Normals
Added Weld Selected Vertices/Edges/Triangles
Added Un-Weld Selected Vertices/Edges/Triangles
Added Join Vertices
Added Join Edges
Added Check For Unused Vertices and Zero Area Triangles
Added Dedicated OBJ Export Function
Updated Rotate/Translate pivot point handles
Added UV and Normal colors to saved settings
Added Custom Skins
Changed Snap To Grid (Removed check boxes and changed to current axis handle only)
Updated selection when switching between unique and coincident vertices/edges 
Fixed Edge selection/deselection when Coincident is off
Changed the extrude key combination from Shift to Ctrl+Shift to avoid creating unwanted edges and triangles when deselecting

v1.5 released 29/09/16
Added: Edge and Triangle splitting now updates tangents by default
Added nstructions on extruding to readme.txt and tutorial

v1.6 released 14/01/17
Added Toolbar
Updated GUI, single column
Added Pages for each settings foldout
Added Custom skin
Added Support for Blend Shapes
Added Check for missing normals and UVs and add if necessary
Added Programmable hotkeys

v1.7 released 09/03/17
General fixes and small changes

v1.8 released 10/06/17
General fixes for latest versions of Unity

v1.9 released 10/11/17
Fix for builds
