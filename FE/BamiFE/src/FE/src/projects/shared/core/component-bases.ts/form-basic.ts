import { DestroyRef, Directive, inject } from "@angular/core";
import { MixinFormBasic } from "../minxin-component-base.ts/form-basic";
import { MixinBase } from "../minxin-component-base.ts/base";

@Directive()
export abstract class FormBasic extends MixinFormBasic(MixinBase(class{})) { 
	
}