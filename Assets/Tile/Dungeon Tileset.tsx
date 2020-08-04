<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.4" tiledversion="1.4.1" name="Dungeon Tileset" tilewidth="16" tileheight="16" tilecount="270" columns="27">
 <image source="Dungeon Tileset.png" width="432" height="160"/>
 <terraintypes>
  <terrain name="floor" tile="113"/>
 </terraintypes>
 <tile id="16">
  <properties>
   <property name="unity:SortingLayer" value="ForeGround"/>
   <property name="unity:SortingOrder" type="int" value="1"/>
  </properties>
 </tile>
 <tile id="43">
  <properties>
   <property name="unity:layer" value="Item"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="2" name="block" x="0" y="0" width="16" height="16"/>
   <object id="3" name="Hitbox" x="-16" y="-16" width="48" height="48">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
     <property name="unity:Tag" value="Item"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="110">
  <properties>
   <property name="unity:Tag" value="Item"/>
   <property name="unity:layer" value="Item"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" name="block" x="0" y="0" width="16" height="16"/>
   <object id="2" name="Hitbox" x="-16" y="-16" width="48" height="48">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="112">
  <properties>
   <property name="unity:layer" value="Item"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="-32" y="-32" width="80" height="80">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="113" terrain=",,,0">
  <objectgroup draworder="index" id="5">
   <object id="10" x="6.14769" y="4.29057">
    <polygon points="0.85231,0.70943 0.85231,11.7094 -6.14769,11.7094 -6.14769,-4.29057 9.85231,-4.29057 9.85231,0.70943"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="114" terrain=",,0,0">
  <objectgroup draworder="index" id="2">
   <object id="6" x="0" y="0" width="16" height="5"/>
  </objectgroup>
 </tile>
 <tile id="115" terrain=",,0,">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="4.55566">
    <polygon points="0,0.44434 9,0.44434 9,11.4443 16,11.4443 16,-4.55566 0,-4.55566"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="130">
  <properties>
   <property name="unity:SortingLayer" value="ForeGround"/>
   <property name="unity:layer" value="Item"/>
  </properties>
  <objectgroup draworder="index" id="4">
   <object id="3" name="Hitbox" x="-16" y="-16" width="48" height="48">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
     <property name="unity:layer" value="Item"/>
    </properties>
   </object>
   <object id="4" name="Block" x="0" y="0" width="16" height="16">
    <properties>
     <property name="unity:layer" value="Item"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="140" terrain=",0,,0">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="7" height="16"/>
  </objectgroup>
 </tile>
 <tile id="141" terrain="0,0,0,0"/>
 <tile id="142" terrain="0,,0,">
  <objectgroup draworder="index" id="3">
   <object id="2" x="9" y="0" width="7" height="16"/>
  </objectgroup>
 </tile>
 <tile id="167" terrain=",0,,">
  <objectgroup draworder="index" id="2">
   <object id="1" x="6.01961" y="0.320192">
    <polygon points="0.98039,-0.320192 0.98039,10.6798 9.98039,10.6798 9.98039,15.6798 -6.01961,15.6798 -6.01961,-0.320192"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="168" terrain="0,0,,">
  <objectgroup draworder="index" id="3">
   <object id="2" x="0" y="11" width="16" height="5.00964"/>
  </objectgroup>
 </tile>
 <tile id="169" terrain="0,,,">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0.320192" y="12.1673">
    <polygon points="-0.320192,-1.1673 8.67981,-1.1673 8.67981,-12.1673 15.6798,-12.1673 15.6798,3.8327 -0.384231,3.90634"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="220">
  <properties>
   <property name="unity:Tag" value="Trap"/>
  </properties>
 </tile>
 <tile id="221">
  <properties>
   <property name="unity:Tag" value="BuffTile"/>
  </properties>
 </tile>
 <tile id="222">
  <properties>
   <property name="unity:Tag" value="Obstacle"/>
  </properties>
 </tile>
 <tile id="246">
  <properties>
   <property name="unity:IsTrigger" type="bool" value="true"/>
   <property name="unity:Tag" value="Item"/>
   <property name="unity:layer" value="Item"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" name="Hitbox" x="-16" y="-16" width="48" height="48"/>
  </objectgroup>
 </tile>
</tileset>
