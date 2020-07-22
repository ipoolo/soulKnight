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
   <object id="3" name="hitbox" x="-16" y="-16" width="48" height="48">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="110">
  <objectgroup draworder="index" id="2">
   <object id="1" name="block" x="0" y="0" width="16" height="16"/>
   <object id="2" name="hitBox" x="-16" y="-16" width="48" height="48">
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
    <polygon points="-0.128077,0 -0.0640384,11.591 -6.08365,11.591 -6.14769,-4.41865 9.85231,-4.29057 9.79788,0.128077"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="114" terrain=",,0,0">
  <objectgroup draworder="index" id="2">
   <object id="1" x="-0.125" y="0.1875" width="16.25" height="4.3125"/>
  </objectgroup>
 </tile>
 <tile id="115" terrain=",,0,">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0.192115" y="4.41865">
    <polygon points="0,0 9.60576,0.128077 9.6698,11.3988 15.6254,11.4629 16.0096,-4.61077 0,-4.41865"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="140" terrain=",0,,0">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0.128077" y="0" width="6.46788" height="16.1377"/>
  </objectgroup>
 </tile>
 <tile id="141" terrain="0,0,0,0"/>
 <tile id="142" terrain="0,,0,">
  <objectgroup draworder="index" id="3">
   <object id="2" x="9.28557" y="0" width="6.66" height="16.0096"/>
  </objectgroup>
 </tile>
 <tile id="167" terrain=",0,,">
  <objectgroup draworder="index" id="2">
   <object id="1" x="6.01961" y="0.320192">
    <polygon points="0.0640384,-0.448269 0.384231,-0.448269 0.448269,10.5663 9.86192,10.6944 9.98999,15.6254 -6.14769,15.7535 -5.89153,-0.448269"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="168" terrain="0,0,,">
  <objectgroup draworder="index" id="3">
   <object id="2" x="0.128077" y="10.9506" width="15.7535" height="5.05904"/>
  </objectgroup>
 </tile>
 <tile id="169" terrain="0,,,">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0.320192" y="12.1673">
    <polygon points="-0.448269,-0.128077 9.79788,-0.128077 9.79788,-12.0392 15.5613,-12.0392 15.8175,3.90634 -0.384231,3.90634"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="246">
  <properties>
   <property name="unity:IsTrigger" type="bool" value="true"/>
   <property name="unity:layer" value="Item"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="-16" y="-16" width="48" height="48"/>
  </objectgroup>
 </tile>
</tileset>
