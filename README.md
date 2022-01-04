# DotNetDocsShow.Nomad
The sample parcel sortation &amp; distribution application Nomad, used on the dotnet docs show, where I talked about using the cosmos repository pattern.

## Business Domain

Nomand is a small distribution center, it aims to allow businesses to ship there parcels to there facilities and then have those parcels routed onto vans heading to differnet regions in the UK. They do this via some complex machinery which efficiently sorts and loads the parcels onto the back of the vans.

## The Application

This application is a phase 1 development of a system to manage the integration with the third party machinery doing the sorting & loading. Parcels are inducted into this system and then once van parks in one of the pre-defined docks if the van is heading to that region then the parcel will be placed into that van for shipping.

### The high level system path

This system starts off by recieving notice of a parcel via a HTTP request from a client of there services. This system then stores this data and waits for the parcel to be inducted into the sorting & loading system, once inducted the system keeps track of the parcel and the record that it was inducted, it will then it will sort the parcels into region specific groups all the parcels that are currently in the system for a given region. Then when a delivery van docks the system will take a note of the region the van is travelling along with a parcel capacity and it will route parcels via the machinery to the given van for dispatch. 

The progress of these parcels through the system should be reported on from a client & region level.
