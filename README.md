# Nomad.Sorter
The sample parcel sortation &amp; distribution application Nomad, used on the dotnet docs show, where I talked about using the cosmos repository pattern.

## Business Domain

Nomand is a small distribution center, it aims to allow businesses to ship there parcels to there facilities and then have those parcels routed onto vans heading to differnet regions in the UK. They do this via some complex machinery which efficiently sorts and loads the parcels onto the back of the vans.

## The Application

This application is a phase 1 development of a system to manage the integration with the third party machinery doing the sorting & loading. Parcels are inducted into this system and then once van parks in one of the pre-defined docks if the van is heading to that region then the parcel will be placed into that van for shipping.

### The high level system path

This system starts off by recieving notice of a parcel via a HTTP request from a client of there services. This system then stores this data and waits for the parcel to be inducted into the sorting & loading system, once inducted the system keeps track of the parcel and the record that it was inducted, it will then it will sort the parcels into region specific groups all the parcels that are currently in the system for a given region. Then when a delivery van docks the system will take a note of the region the van is travelling along with a parcel capacity and it will route parcels via the machinery to the given van for dispatch. 

The progress of these parcels through the system should be reported on from a client & region level.

### A Parcel's Journey

This section details a parcel's journey through the system. See the diargam below, take note of the step numbers, these are explained in the next section. It is worth noting the system that this repository contains is the one in the center the `Nomad.Sorter`

![Nomad Demo Solution (3)](https://user-images.githubusercontent.com/23740684/148514321-67fd9474-6c84-4660-a6e9-c78e4ab59ea5.png)

#### Preamble

The first thing that happens in the system is an external customer who is wanting to use `Nomad` to deliver there parcels will send Nomad a pre-advice of a parcel they plan to deliver to one of Nomad's distribution centers. Here a few checks happen on the client's service (`Nomad.Clients`) to make sure they are a valid client & then the `Nomad.Router` will provide a `deliveryRegionId`.


#### Step 1 - Parcel Pre-Advice

Once the client service and router have done there work a message will be placed onto a queue called `nomad-sorter/parcel-pre-advice`. The `Nomad.Sorter` will listen for this message and then import the data into it's database. It will then wait for the parcel to arrive.

#### Step 2 - Parcel Inducted

The next step in the parcel's jounrey occurs when the parcel arrives at a Nomad distribution center and is inducted into the sorting system. At this point the `Third Party Machinery` will raise an event to notify any systems of a parcel being inducted. This will place an event onto the topic `tpm/parcel-inducted` (`tpm` = third party machinery). The `Nomad.Sorter` will now update that parcel's status to be `Inducted`. It will now wait for a vehicle to dock in order to associate the parcel with a delivery run.


