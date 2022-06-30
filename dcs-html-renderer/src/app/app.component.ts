//BEGIN LICENSE BLOCK 
//Interneuron Synapse

//Copyright(C) 2022  Interneuron CIC

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
//END LICENSE BLOCK 
import { Component, OnInit } from '@angular/core';
import { DcsHtmlRendererService } from '../../projects/dcs-html-renderer/src/public-api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'interneuron-dcs-renderer';
  formResponse: any;

  constructor(private dcsHtmlRendererService: DcsHtmlRendererService) { }

  ngOnInit() {
    this.generateForm();
  }

  generateForm() {
    let formJson = {
      "form": {
        "forminstance_id": "7777746d-ac6d-40e5-be86-5c7c2138f219",
        "form_id": "2",
        "formname": "Pre-Assessment Health Questionnaire",
        "description": "Please complete the following questionnaire about your health. Your answers will help us provide the best care for you. If you have any questions, please ask a staff member.",
        "formparameters": [
          {
            "parametername": "Person_Id",
            "parameterkey": "personId",
            "parametervalue": "per1"
          },
          {
            "parametername": "Encounter_Id",
            "parameterkey": "encounterId",
            "parametervalue": "enc1"
          },
          {
            "parametername": "Form_Context",
            "parameterkey": "observationid",
            "parametervalue": "obs1"
          }
        ],
        "destinationentities": [],
        "formsections": [
          {
            "formsection_id": "section1",
            "title": "1. Heart Problems",
            "displayorder": 1,
            "sectionfields": [
              {
                "field_id": "ghc1",
                "fieldcontroltypename": "ghc",
                "fieldlabeltext": "Smiley",
                "fieldvalue": [
                  "<img src=\"https://www.w3schools.com/tags/smiley.gif\" alt=\"Smiley face\" width=\"42\" height=\"42\">"
                ],
                "fielddisplayorder": -1,
                "fieldvalidations": [
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl1",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "No"
                          ],
                          "order": 1,
                          "logicoperator": "and"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl1",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you have high blood pressure or take medications to control your blood pressure?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 1,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rbl2",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have you had a heart attack?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 2,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required."
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rbl3",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Was it in the past 12 months?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 3,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl2",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "and"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl4",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you suffer from chest pains or chest tightness?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 4,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rbl5",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Does it occur when you are resting?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 5,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl4",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "and"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl6",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Does it occur when you are exerting yourself?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 6,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl4",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "and"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl7",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have you been told you have angina?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 7,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl4",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "and"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl8",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you feel short of breath when you are resting or with light activity?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 8,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rbl9",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you get short of breath after one flight of stairs?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 9,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl8",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "and"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl10",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have you had cardiac bypass surgery or had heart stents inserted?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 10,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rbl11",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Was it in the past 12 months?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 11,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl10",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "and"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl12",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have you ever been treated for or been told you have heart failure?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 12,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rbl13",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have you ever been treated for or been told you have pulmonary hypertension?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 13,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rbl14",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you ever wake at night gasping for breath for no reason?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 14,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rbl15",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Are you known to have a heart murmur, a problem with the valves in your heart, or had a valve replacement?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 15,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rbl16",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you suffer from palpitations or been told you have an irregular heartbeat?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 16,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rbl17",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you have a pacemaker?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 17,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rbl18",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you have an implantable defibrillator (a more complex form of pacemaker which can deliver shocks)?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 18,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rbl19",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you suffer from dizziness or blackouts?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 19,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              }
            ]
          },
          {
            "formsection_id": "section2",
            "title": "2. Breathing Problems",
            "displayorder": 2,
            "sectionfields": [
              {
                "field_id": "rbl20",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you have asthma or chronic obstructive pulmonary disease (COPD)?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 1,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ]
              },
              {
                "field_id": "rbl21",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "If you have asthma, do you have more than one attack per month?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 2,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl20",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl22",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "If you have COPD, have you needed antibiotics +/- steroid tablets more than 3 times in the last year?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 3,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl20",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl23",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you use home nebulisers?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 4,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl20",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "and"
                        },
                        {
                          "field_id": "rbl21",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        },
                        {
                          "field_id": "rbl22",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl24",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you use home oxygen?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 5,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl20",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "and"
                        },
                        {
                          "field_id": "rbl21",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        },
                        {
                          "field_id": "rbl22",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl25",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have you been admitted to hospital because of your breathing in the past year?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 6,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl20",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "and"
                        },
                        {
                          "field_id": "rbl21",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        },
                        {
                          "field_id": "rbl22",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl26",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have you been admitted to an intensive care unit because of your breathing in the past?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 7,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl20",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "and"
                        },
                        {
                          "field_id": "rbl21",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        },
                        {
                          "field_id": "rbl22",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl27",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you have any other breathing/lung conditions (eg lung fibrosis or asbestos lung disease)?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 8,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "tb1",
                "fieldcontroltypename": "tb",
                "fieldlabeltext": "Please specify:",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 8,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl27",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "and"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl28",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have you been told you snore heavily at night?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 9,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rbl29",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have you been told you seem to stop breathing during the night?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 10,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rbl30",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you drop off to sleep frequently during the day or get regular headaches?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 11,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl28",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        },
                        {
                          "field_id": "rbl29",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl31",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have you been told you have obstructive sleep apnoea (OSA)?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 12,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl28",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        },
                        {
                          "field_id": "rbl29",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rbl32",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you use a CPAP mask at night?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 13,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rbl28",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        },
                        {
                          "field_id": "rbl29",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              }
            ]
          },
          {
            "formsection_id": "section3",
            "title": "3. Diabetes, kidney, liver and thyroid problems",
            "displayorder": 3,
            "sectionfields": [
              {
                "field_id": "rblDiabeticMedication",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Are you diabetic requiring medication?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 1,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rblDiabeticMedicationOptions",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "If Yes, select",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 2,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "None",
                    "optiontext": "None"
                  },
                  {
                    "optionval": "Tablets only",
                    "optiontext": "Tablets only"
                  },
                  {
                    "optionval": "Insulin only",
                    "optiontext": "Insulin only"
                  },
                  {
                    "optionval": "insulin & tablets",
                    "optiontext": "insulin & tablets"
                  },
                  {
                    "optionval": "insulin pump",
                    "optiontext": "insulin pump"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rblDiabeticMedication",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rblDiabetesWellControlled",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Is your diabetes well controlled?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 3,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rblDaytimeBM",
                "fieldcontroltypename": "tb",
                "fieldlabeltext": "What has been your average daytime BM in last 2 weeks if you measure it?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 4,
                "disabled": false
              },
              {
                "field_id": "rblDiabeticKetoacidosis",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you have a history of diabetic ketoacidosis (very high blood sugar needing hospital admission) or frequent hypoglycaemic (low blood sugar) episodes?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 5,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rblKidneyOrUrinary",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you have any kidney or urinary problems?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 6,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rblKidneyDialysis",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Are you on kidney dialysis?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 7,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rblThyroid",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you have an overactive or underactive thyroid gland in your neck",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 8,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rblLiverProblems",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you have any liver problems requiring treatment?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 9,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              }
            ]
          },
          {
            "formsection_id": "section4",
            "title": "4. Blood problems",
            "displayorder": 4,
            "sectionfields": [
              {
                "field_id": "rblBloodDisorder",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you have a blood disorder or a condition that makes you prone to bleeding/bruising easily?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 1,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "chkblBloodDisorderOptions",
                "fieldcontroltypename": "chkbl",
                "fieldlabeltext": "If Yes, please select",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 2,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "haemophilia",
                    "optiontext": "haemophilia"
                  },
                  {
                    "optionval": "Von Willebrands disease",
                    "optiontext": "Von Willebrands disease"
                  },
                  {
                    "optionval": "myeloma",
                    "optiontext": "myeloma"
                  },
                  {
                    "optionval": "leukaemia",
                    "optiontext": "leukaemia"
                  },
                  {
                    "optionval": "lymphoma",
                    "optiontext": "lymphoma"
                  },
                  {
                    "optionval": "sickle cell disease",
                    "optiontext": "sickle cell disease"
                  },
                  {
                    "optionval": "thalasseaemia",
                    "optiontext": "thalasseaemia"
                  },
                  {
                    "optionval": "haemochromatosis",
                    "optiontext": "haemochromatosis"
                  },
                  {
                    "optionval": "polycythaemia",
                    "optiontext": "polycythaemia"
                  },
                  {
                    "optionval": "Other",
                    "optiontext": "Other"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rblBloodDisorder",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "tbBloodDisorderOptionOther",
                "fieldcontroltypename": "tb",
                "fieldlabeltext": "Please specify",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 3,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "chkblBloodDisorderOptions",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Other"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rblAnaemic",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Are you known to be anaemic?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 4,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rblBloodClot",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have you had a blood clot in your lung or legs?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 5,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rblBloodTransfusion",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Would you accept a transfusion of blood if your surgeon thinks it would be in your best interest?",
                "fieldvalue": [
                  "Yes"
                ],
                "fielddisplayorder": 6,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              }
            ]
          },
          {
            "formsection_id": "section5",
            "title": "5. Brain, nerve, muscle and bone problems",
            "displayorder": 5,
            "sectionfields": [
              {
                "field_id": "rblStroke",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have you ever had a stroke or mini stroke?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 1,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rblStrokeSixMonths",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Was it within the past 6 months",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 2,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rblStroke",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rblEpilepsy",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you have epilepsy?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 3,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rblFitPerMonth",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Is it difficult to control e.g. more than 1 fit per month?",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 4,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rblEpilepsy",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rblBNM",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you have any other brain/nerve/muscle disease?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 5,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "chkblBNMOptions",
                "fieldcontroltypename": "chkbl",
                "fieldlabeltext": "If Yes, please select",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 6,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "multiple sclerosis",
                    "optiontext": "multiple sclerosis"
                  },
                  {
                    "optionval": "a muscular dystrophy",
                    "optiontext": "a muscular dystrophy"
                  },
                  {
                    "optionval": "friedrich ataxia",
                    "optiontext": "friedrich ataxia"
                  },
                  {
                    "optionval": "Guillain Barre",
                    "optiontext": "Guillain Barre"
                  },
                  {
                    "optionval": "myasthenia gravis",
                    "optiontext": "myasthenia gravis"
                  },
                  {
                    "optionval": "Other",
                    "optiontext": "Other"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rblBNM",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "tbBNMOptionOther",
                "fieldcontroltypename": "tb",
                "fieldlabeltext": "Please specify",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 7,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "chkblBNMOptions",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Other"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rblDementia",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have you been diagnosed with dementia?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 8,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rblMHC",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Are you currently being treated for any mental health condition?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 9,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "tbMHCSpecify",
                "fieldcontroltypename": "tb",
                "fieldlabeltext": "If yes, please specify",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 10,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rblMHC",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rblArthritis",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have you been told you have rheumatoid arthritis (different to osteoarthritis)?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 11,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rblNJMovements",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you have any restrictions of your neck or jaw movement?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 12,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              }
            ]
          },
          {
            "formsection_id": "section6",
            "title": "6. Previous Anaesthetics Problems",
            "displayorder": 6,
            "sectionfields": [
              {
                "field_id": "rblProbWithGenAnae",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have you or a family member had any problems with or allergic or anaphylactic reactions to general anaesthetics",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 1,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rblBreathingTube",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Has an anaesthetist told you that your airway was difficult to manage or it was difficult to pass a breathing tube?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 2,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              }
            ]
          },
          {
            "formsection_id": "section7",
            "title": "7. Pain Assessment",
            "displayorder": 7,
            "sectionfields": [
              {
                "field_id": "rblChronicPainClinic",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Are you seen in a chronic pain clinic?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 1,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rblMorphine",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you take regular morphine or oxycodone or similar?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 2,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "rblPostOperativePain",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Have had problems with pain postoperatively in the past?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 3,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              }
            ]
          },
          {
            "formsection_id": "section8",
            "title": "8. Other Questions",
            "displayorder": 8,
            "sectionfields": [
              {
                "field_id": "rblOtherHosp",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Are you receiving treatment from another hospital or have any other significant medical conditions not already mentioned?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 1,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "tbOtherHospDetails",
                "fieldcontroltypename": "tb",
                "fieldlabeltext": "If yes, please give details",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 2,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rblOtherHosp",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "rblCongenital",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Do you have a syndromic / congenital condition (eg achondroplasia, mucopolysaccharidosis, Down’s syndrome) not already mentioned?",
                "fieldvalue": [
                  "No"
                ],
                "fielddisplayorder": 3,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "tbContenitalDetails",
                "fieldcontroltypename": "tb",
                "fieldlabeltext": "If yes, please give details",
                "fieldvalue": [
                  ""
                ],
                "fielddisplayorder": 4,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "displayrules": {
                  "conditiongroup": [
                    {
                      "condition": [
                        {
                          "field_id": "rblCongenital",
                          "compareoperator": "equalsto",
                          "comparevalue": [
                            "Yes"
                          ],
                          "order": 1,
                          "logicoperator": "or"
                        }
                      ],
                      "logicoperator": "and"
                    }
                  ]
                },
                "disabled": false
              },
              {
                "field_id": "chkblAnyMeds",
                "fieldcontroltypename": "chkbl",
                "fieldlabeltext": "Please select any of the following medications you are taking?",
                "fieldvalue": [
                  "Warfarin",
                  "Dabigitran",
                  "Apixiban"
                ],
                "fielddisplayorder": 5,
                "optionlist": [
                  {
                    "optionval": "Contraceptive pills",
                    "optiontext": "Contraceptive pills"
                  },
                  {
                    "optionval": "Hormone Replacement Therapy (HRT)",
                    "optiontext": "Hormone Replacement Therapy (HRT)"
                  },
                  {
                    "optionval": "Aspirin",
                    "optiontext": "Aspirin"
                  },
                  {
                    "optionval": "Warfarin",
                    "optiontext": "Warfarin"
                  },
                  {
                    "optionval": "Clopidogrel",
                    "optiontext": "Clopidogrel"
                  },
                  {
                    "optionval": "Dabigitran",
                    "optiontext": "Dabigitran"
                  },
                  {
                    "optionval": "Apixiban",
                    "optiontext": "Apixiban"
                  },
                  {
                    "optionval": "Rivaroxiban",
                    "optiontext": "Rivaroxiban"
                  }
                ],
                "disabled": true
              },
              {
                "field_id": "rblGPRecords",
                "fieldcontroltypename": "rbl",
                "fieldlabeltext": "Are you happy for us to view your GP medication records (Summary Care Record)?",
                "fieldvalue": [
                  "Yes"
                ],
                "fielddisplayorder": 6,
                "fieldvalidations": [
                  {
                    "validationtype": "req",
                    "validationerrormsg": "Required"
                  }
                ],
                "optionlist": [
                  {
                    "optionval": "Yes",
                    "optiontext": "Yes"
                  },
                  {
                    "optionval": "No",
                    "optiontext": "No"
                  }
                ],
                "disabled": false
              },
              {
                "field_id": "taOtherMeds",
                "fieldcontroltypename": "ta",
                "fieldlabeltext": "List all other medications you are taking or write \"NONE\"",
                "fieldvalue": [
                  "Nope"
                ],
                "fielddisplayorder": 7,
                "disabled": true
              },
              {
                "field_id": "taDrugAllergies",
                "fieldcontroltypename": "ta",
                "fieldlabeltext": "List all Drug Allergies you are having or write \"NONE\"",
                "fieldvalue": [
                  "Nope"
                ],
                "fielddisplayorder": 8,
                "disabled": false
              },
              {
                "field_id": "taOtherAllergies",
                "fieldcontroltypename": "ta",
                "fieldlabeltext": "List all other Allergies you are having or write \"NONE\"",
                "fieldvalue": [
                  "Nope"
                ],
                "fielddisplayorder": 9,
                "disabled": false
              }
            ]
          }
        ]
      }
    };

    this.dcsHtmlRendererService.formJson.next(formJson);

    this.dcsHtmlRendererService.formResponseJson.subscribe(
      (formResponseJson: any) => {
        this.formResponse = formResponseJson;

        console.log(this.formResponse);
      }
    );
  }
}
