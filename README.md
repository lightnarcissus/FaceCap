# FaceCap
markerless facial animation capture and real-time viseme detection

## Architecture 

* Uses OpenCV and dlib in conjunction with OpenFace to use 68-point facial landmark detection to detect Action Units based on the FACS model. 
* Use ZeroMQ to communicate the Action Unit (AU) intensity to Unity3D/Unreal Engine 4 project for real-time demonstration or via a PyMEL pipeline to Maya for keyframe recording
**(TODO)**
* **Use RNNNoise to denoise microphone input and feed it to VisemeNet to extract the relevant visemes. Apply it to a Daz3D Genesis 3 model in Maya via the PyMEL pipeline.**


## License
This repository is dependent upon [OpenFace](https://github.com/TadasBaltrusaitis/OpenFace) and [VisemeNet](https://github.com/yzhou359/VisemeNet_tensorflow/) and the licenses of both should be respected if you end up using anything from this repository.
