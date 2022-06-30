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
import { Form } from '../model/Form';
import { ErrorHandlerService } from '../services/error-handler.service';
import { FormHelpers } from "../components/form-model";
import { DcsHtmlRendererService } from './dcs-html-renderer.service';
import { ElementType } from '../enumeration/element-type';

@Component({
  selector: 'lib-dcs-html-renderer',
  templateUrl: './dcs-html-renderer.component.html',
  styles: []
})
export class DcsHtmlRendererComponent implements OnInit {
  private form: Form;

  constructor(
    private errorHandlerService: ErrorHandlerService,
    private dcsHtmlRendererService: DcsHtmlRendererService) {
    this.subscribeEvents();
  }

  subscribeEvents(): any {
    this.dcsHtmlRendererService.formJson.subscribe(
      (json: string) => {
        this.form = FormHelpers.deserialize(json);
      },
      error => this.errorHandlerService.handleError(error)
    );
  }

  ngOnInit() {
    this.createForm();
  }

  createForm(): void {
    if (this.form != null && this.form != undefined) {
      var formDiv = document.getElementById("form");
      formDiv.append(this.form.getMarkup());
    }
  }

  showSubmitButton() {
    return this.form == null && this.form == undefined;
  }

  submitForm() {
    var formData: any = new Object();
    var doSubmit = true;

    formData.form_id = this.form.id;
    formData.forminstance_id = this.form.forminstance_id;
    formData.formparameters = this.form.formparameters;
    formData.destinationentities = this.form.destinationentities;
    formData.formsections = [];

    for (var i = 0; i < this.form.sections.length; i++) {
      var formSection: any = new Object();
      formSection.section_id = this.form.sections[i].id;
      formSection.title = this.form.sections[i].title;
      formSection.sectionfields = [];
      for (var j = 0; j < this.form.sections[i].elements.length; j++) {
        if (this.form.sections[i].elements[j].getControlType() == ElementType.GenericHTMLControl)
          continue;
        var sectionField: any = new Object();
        sectionField.field_id = this.form.sections[i].elements[j].id;
        sectionField.fieldcontroltypename = this.form.sections[i].elements[j].getControlType();
        sectionField.fieldlabeltext = this.form.sections[i].elements[j].labelText;
        sectionField.fielddata = this.form.sections[i].elements[j].getValue();

        sectionField.fieldvalidation = this.form.sections[i].elements[j].validate();
        doSubmit = doSubmit && sectionField.fieldvalidation.result;

        formSection.sectionfields.push(sectionField);
      }
      formData.formsections.push(formSection);
    }

    this.dcsHtmlRendererService.formResponseJson.next(formData);
  }
}
