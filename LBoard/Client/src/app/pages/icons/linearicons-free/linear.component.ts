﻿import { Component } from '@angular/core'
declare var require: any
const data: any = require('./data.json')

@Component({
  selector: 'app-icons-linear',
  templateUrl: './linear.component.html',
})
export class IconsLinearComponent {
  icons = data
}
