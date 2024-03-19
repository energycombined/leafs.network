# leafs.network
Open-source laboratory platform - initially targeted at battery research - possibly interesting for other materials sciences. 
It is currently tested in the SIMBA consortium. https://simba-h2020.eu/project-introduction/
Public experiments will appear here: https://leafs.network/Experiments/PublicExperiments

LEAFS allows to enter all material details, denote its material function in the battery (e.g. active, conductor, binder, pore former), keep track of a material inventory and store, share and visualize battery results.
Once files from battery testers (e.g. Maccor, Arbin) are uploaded, it is send to a FLASK server that runs CELLPY (https://github.com/jepegit/cellpy), converts the data and sends it back in the right format to be visualised in the webbrowser of LEAFS.

