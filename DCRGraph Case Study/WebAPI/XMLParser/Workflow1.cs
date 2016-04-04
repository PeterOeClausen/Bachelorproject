﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.XMLParser
{
    class Workflow1
    {

        public string getflow()
        {
            string xmlFilepath = @"C:/Bachelor2.xml";
            String line;
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(xmlFilepath))
            {
                
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            return line;
        }

        public String Workflow =
@"<dcrgraph title='Bachelor 2' dataTypesStatus='hide' filterLevel='4' zoomLevel='-4'>
    <specification>
        <resources>
            <events>
                <event id='Activity 0' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='191' yLoc='478' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>1</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 3' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='692' yLoc='374' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>1</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 4' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='686' yLoc='218' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>1</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 5' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='954' yLoc='282' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>1</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 6' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='1083' yLoc='102' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>1</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 7' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='1129' yLoc='264' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>1</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 8' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='1121' yLoc='427' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>1</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 9' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='1287' yLoc='94' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>1</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 10' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='1486' yLoc='306' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>1</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 11' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='1483' yLoc='85' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>1</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 13' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='1298' yLoc='806' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>Edit events</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>3</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 15' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='995' yLoc='775' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>Edit events</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>1</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 16' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='878' yLoc='775' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>Edit events</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>1</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 19' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='1409' yLoc='805' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>Edit events</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>3</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 20' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='547' yLoc='53' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>4</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 21' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='701' yLoc='30' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>4</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 22' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='397' yLoc='75' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>4</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 23' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='202' yLoc='68' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>4</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 24' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='88' yLoc='100' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>4</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 25' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='811' yLoc='549' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>only pending</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>1</level>
                        <eventData></eventData>
                    </custom>
                </event>
                <event id='Activity 26' scope='private' >
                    <custom>
                        <visualization>
                            <location xLoc='1529' yLoc='811' />
                        </visualization>
                        <roles>
                            <role></role>
                        </roles>
                        <groups>
                            <group>Edit events</group>
                        </groups>
                        <eventType></eventType>
                        <eventDescription></eventDescription>
                        <level>3</level>
                        <eventData></eventData>
                    </custom>
                </event>
            </events>
            <subProcesses></subProcesses>
            <distribution>
                <externalEvents></externalEvents>
            </distribution>
            <labels>
                <label id='Confirm web order' />
                <label id='Cook order to eat in restaurant' />
                <label id='Cook order for delivery or pickup' />
                <label id='Done cooking' />
                <label id='Serve order to table' />
                <label id='Picked up' />
                <label id='Deliver order' />
                <label id='Clean table' />
                <label id='Done' />
                <label id='Pay' />
                <label id='Change to takeaway' />
                <label id='Remove item from order' />
                <label id='Add item to order' />
                <label id='Change to delivery' />
                <label id='Setup graph web takeaway' />
                <label id='Setup graph web delivery' />
                <label id='Setup graph delivery' />
                <label id='Setup graph serving' />
                <label id='Setup graph takeaway' />
                <label id='confirm changes' />
                <label id='Change to serve' />
            </labels>
            <labelMappings>
                <labelMapping eventId='Activity 0' labelId='Confirm web order'/>
                <labelMapping eventId='Activity 3' labelId='Cook order to eat in restaurant'/>
                <labelMapping eventId='Activity 4' labelId='Cook order for delivery or pickup'/>
                <labelMapping eventId='Activity 5' labelId='Done cooking'/>
                <labelMapping eventId='Activity 6' labelId='Serve order to table'/>
                <labelMapping eventId='Activity 7' labelId='Picked up'/>
                <labelMapping eventId='Activity 8' labelId='Deliver order'/>
                <labelMapping eventId='Activity 9' labelId='Clean table'/>
                <labelMapping eventId='Activity 10' labelId='Done'/>
                <labelMapping eventId='Activity 11' labelId='Pay'/>
                <labelMapping eventId='Activity 13' labelId='Change to takeaway'/>
                <labelMapping eventId='Activity 15' labelId='Remove item from order'/>
                <labelMapping eventId='Activity 16' labelId='Add item to order'/>
                <labelMapping eventId='Activity 19' labelId='Change to delivery'/>
                <labelMapping eventId='Activity 20' labelId='Setup graph web takeaway'/>
                <labelMapping eventId='Activity 21' labelId='Setup graph web delivery'/>
                <labelMapping eventId='Activity 22' labelId='Setup graph delivery'/>
                <labelMapping eventId='Activity 23' labelId='Setup graph serving'/>
                <labelMapping eventId='Activity 24' labelId='Setup graph takeaway'/>
                <labelMapping eventId='Activity 25' labelId='confirm changes'/>
                <labelMapping eventId='Activity 26' labelId='Change to serve'/>
            </labelMappings>
            <expressions></expressions>
            <variables></variables>
            <variableAccesses>
                <writeAccesses />
            </variableAccesses>
            <custom>
                <roles></roles>
                <groups>
                    <group sequence='0'>always</group>
                    <group sequence='0'>Edit events</group>
                    <group sequence='0'>only pending</group>
                    <group sequence='0'>Waiter serves order</group>
                </groups>
                <eventTypes></eventTypes>
                <graphDetails></graphDetails>
                <graphFilters>
                    <filteredGroups></filteredGroups>
                    <filteredRoles></filteredRoles>
                </graphFilters>
            </custom>
        </resources>
        <constraints>
            <conditions>
                <condition sourceId='Activity 0' targetId='Activity 4' filterLevel='1'  description='  time='  groups='  />
            </conditions>
            <responses>
                <response sourceId='Activity 0' targetId='Activity 4' filterLevel='1'  description='  time='  groups='  />
                <response sourceId='Activity 3' targetId='Activity 5' filterLevel='1'  description='  time='  groups='  />
                <response sourceId='Activity 4' targetId='Activity 5' filterLevel='1'  description='  time='  groups='  />
                <response sourceId='Activity 5' targetId='Activity 6' filterLevel='1'  description='  time='  groups='  />
                <response sourceId='Activity 5' targetId='Activity 7' filterLevel='1'  description='  time='  groups='  />
                <response sourceId='Activity 5' targetId='Activity 8' filterLevel='1'  description='  time='  groups='  />
                <response sourceId='Activity 6' targetId='Activity 9' filterLevel='1'  description='  time='  groups='  />
                <response sourceId='Activity 7' targetId='Activity 10' filterLevel='1'  description='  time='  groups='  />
                <response sourceId='Activity 8' targetId='Activity 10' filterLevel='1'  description='  time='  groups='  />
                <response sourceId='Activity 9' targetId='Activity 10' filterLevel='1'  description='  time='  groups='  />
                <response sourceId='Activity 21' targetId='Activity 0' filterLevel='4'  description='  time='  groups='  />
                <response sourceId='Activity 22' targetId='Activity 4' filterLevel='4'  description='  time='  groups='  />
                <response sourceId='Activity 22' targetId='Activity 11' filterLevel='4'  description='  time='  groups='  />
                <response sourceId='Activity 23' targetId='Activity 3' filterLevel='4'  description='  time='  groups='  />
                <response sourceId='Activity 23' targetId='Activity 4' filterLevel='4'  description='  time='  groups='  />
                <response sourceId='Activity 23' targetId='Activity 11' filterLevel='4'  description='  time='  groups='  />
                <response sourceId='Activity 24' targetId='Activity 4' filterLevel='4'  description='  time='  groups='  />
                <response sourceId='Activity 24' targetId='Activity 11' filterLevel='4'  description='  time='  groups='  />
                <response sourceId='Activity 15' targetId='Activity 25' filterLevel='3'  description='  time='  groups='  />
                <response sourceId='Activity 16' targetId='Activity 25' filterLevel='3'  description='  time='  groups='  />
                <response sourceId='Activity 25' targetId='Activity 3' filterLevel='1'  description='  time='  groups='  />
                <response sourceId='Activity 25' targetId='Activity 4' filterLevel='1'  description='  time='  groups='  />
            </responses>
            <excludes>
                <exclude sourceId='Activity 0' targetId='Activity 0' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 20' targetId='Activity 3' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 20' targetId='Activity 6' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 20' targetId='Activity 8' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 20' targetId='Activity 9' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 20' targetId='Activity 11' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 20' targetId='Activity 20' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 20' targetId='Activity 21' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 20' targetId='Activity 22' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 20' targetId='Activity 23' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 20' targetId='Activity 24' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 21' targetId='Activity 3' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 21' targetId='Activity 6' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 21' targetId='Activity 7' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 21' targetId='Activity 9' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 21' targetId='Activity 11' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 21' targetId='Activity 20' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 21' targetId='Activity 21' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 21' targetId='Activity 22' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 21' targetId='Activity 23' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 21' targetId='Activity 24' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 22' targetId='Activity 0' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 22' targetId='Activity 3' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 22' targetId='Activity 6' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 22' targetId='Activity 7' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 22' targetId='Activity 9' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 22' targetId='Activity 20' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 22' targetId='Activity 21' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 22' targetId='Activity 22' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 22' targetId='Activity 23' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 23' targetId='Activity 0' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 23' targetId='Activity 4' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 23' targetId='Activity 7' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 23' targetId='Activity 8' filterLevel='1'  description='  time='  groups='only pending'  />
                <exclude sourceId='Activity 23' targetId='Activity 21' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 23' targetId='Activity 22' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 23' targetId='Activity 23' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 23' targetId='Activity 24' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 24' targetId='Activity 0' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 24' targetId='Activity 3' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 24' targetId='Activity 6' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 24' targetId='Activity 8' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 24' targetId='Activity 9' filterLevel='1'  description='  time='  groups='only pending'  />
                <exclude sourceId='Activity 24' targetId='Activity 20' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 24' targetId='Activity 21' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 24' targetId='Activity 22' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 24' targetId='Activity 23' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 24' targetId='Activity 24' filterLevel='4'  description='  time='  groups='  />
                <exclude sourceId='Activity 19' targetId='Activity 6' filterLevel='3'  description='  time='  groups='  />
                <exclude sourceId='Activity 13' targetId='Activity 6' filterLevel='3'  description='  time='  groups='  />
                <exclude sourceId='Activity 19' targetId='Activity 9' filterLevel='3'  description='  time='  groups='  />
                <exclude sourceId='Activity 13' targetId='Activity 9' filterLevel='3'  description='  time='  groups='  />
                <exclude sourceId='Activity 19' targetId='Activity 7' filterLevel='3'  description='  time='  groups='  />
                <exclude sourceId='Activity 13' targetId='Activity 8' filterLevel='3'  description='  time='  groups='  />
                <exclude sourceId='Activity 26' targetId='Activity 8' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 26' targetId='Activity 7' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 19' targetId='Activity 3' filterLevel='3'  description='  time='  groups='  />
                <exclude sourceId='Activity 13' targetId='Activity 3' filterLevel='3'  description='  time='  groups='  />
                <exclude sourceId='Activity 26' targetId='Activity 4' filterLevel='1'  description='  time='  groups='  />
                <exclude sourceId='Activity 25' targetId='Activity 25' filterLevel='1'  description='  time='  groups='  />
            </excludes>
            <includes>
                <include sourceId='Activity 13' targetId='Activity 7' filterLevel='3'  description='  time='  groups='  />
                <include sourceId='Activity 19' targetId='Activity 8' filterLevel='3'  description='  time='  groups='  />
                <include sourceId='Activity 26' targetId='Activity 6' filterLevel='1'  description='  time='  groups='  />
                <include sourceId='Activity 26' targetId='Activity 9' filterLevel='1'  description='  time='  groups='  />
                <include sourceId='Activity 19' targetId='Activity 4' filterLevel='3'  description='  time='  groups='  />
                <include sourceId='Activity 13' targetId='Activity 4' filterLevel='3'  description='  time='  groups='  />
                <include sourceId='Activity 26' targetId='Activity 3' filterLevel='1'  description='  time='  groups='  />
                <include sourceId='Activity 16' targetId='Activity 25' filterLevel='1'  description='  time='  groups='  />
                <include sourceId='Activity 15' targetId='Activity 25' filterLevel='1'  description='  time='  groups='  />
            </includes>
            <milestones>
                <milestone sourceId='Activity 4' targetId='Activity 5' filterLevel='1'  description='  time='  groups='  />
                <milestone sourceId='Activity 3' targetId='Activity 5' filterLevel='1'  description='  time='  groups='  />
                <milestone sourceId='Activity 5' targetId='Activity 6' filterLevel='1'  description='  time='  groups='  />
                <milestone sourceId='Activity 5' targetId='Activity 7' filterLevel='1'  description='  time='  groups='  />
                <milestone sourceId='Activity 5' targetId='Activity 8' filterLevel='1'  description='  time='  groups='  />
                <milestone sourceId='Activity 6' targetId='Activity 9' filterLevel='1'  description='  time='  groups='  />
                <milestone sourceId='Activity 7' targetId='Activity 10' filterLevel='1'  description='  time='  groups='  />
                <milestone sourceId='Activity 8' targetId='Activity 10' filterLevel='1'  description='  time='  groups='  />
                <milestone sourceId='Activity 9' targetId='Activity 10' filterLevel='1'  description='  time='  groups='  />
                <milestone sourceId='Activity 11' targetId='Activity 7' filterLevel='1'  description='  time='  groups='  />
                <milestone sourceId='Activity 11' targetId='Activity 10' filterLevel='1'  description='  time='  groups='  />
                <milestone sourceId='Activity 25' targetId='Activity 3' filterLevel='1'  description='  time='  groups='  />
            </milestones>
            <spawns></spawns>
        </constraints>
    </specification>
    <runtime>
        <marking>
            <globalStore></globalStore>
            <executed></executed>
            <included>
                <event id='Activity 0'/>
                <event id='Activity 3'/>
                <event id='Activity 4'/>
                <event id='Activity 5'/>
                <event id='Activity 6'/>
                <event id='Activity 7'/>
                <event id='Activity 8'/>
                <event id='Activity 9'/>
                <event id='Activity 10'/>
                <event id='Activity 11'/>
                <event id='Activity 13'/>
                <event id='Activity 15'/>
                <event id='Activity 16'/>
                <event id='Activity 19'/>
                <event id='Activity 20'/>
                <event id='Activity 21'/>
                <event id='Activity 22'/>
                <event id='Activity 23'/>
                <event id='Activity 24'/>
                <event id='Activity 26'/>
            </included>
            <pendingResponses>
                <event id='Activity 11'/>
                <event id='Activity 20'/>
                <event id='Activity 21'/>
                <event id='Activity 22'/>
                <event id='Activity 23'/>
                <event id='Activity 24'/>
            </pendingResponses>
        </marking>
        <custom />
    </runtime>
</dcrgraph>";

    }
}
