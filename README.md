# FaceCap
markerless facial animation capture and real-time viseme detection

## Architecture

* Uses OpenCV and dlib in conjunction with OpenFace to use 68-point facial landmark detection to detect Action Units based on the FACS model. 
* Use ZeroMQ to communicate the Action Unit (AU) intensity to Unity3D/Unreal Engine 4 project for real-time demonstration or via a PyMEL pipeline to Maya for keyframe recording
* Use PocketSphinx to get phonemes from a recording microphone and feed it to VisemeNet to extract the relevant visemes. Apply it to the model in Maya via the PyMEL pipeline.

