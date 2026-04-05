import { Component, OnInit } from '@angular/core';
import { FormDialog } from '@shared/core/component-bases.ts/form-dialog';
import { RoleService } from '../../../services/role.service';
import { IEventSubmitForm } from '@mylib-shared/interfaces/base.interface';
import { required } from '@shared/validators/validator-common';
import { IPermissionConfig } from '@mylib-shared/models/base.model';
import { IPermissionTreeItem } from '../../../models/base.models';
import { ContextMenuService, TreeNode } from 'primeng/api';
import { Utils } from '@mylib-shared/utils';
import { IRole, IRoleUpdate } from '../../../models/role.model';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-role-form',
  templateUrl: './role-form.component.html',
  styleUrls: ['./role-form.component.scss'],
  providers: [ContextMenuService]
})
export class RoleFormComponent extends FormDialog implements OnInit {

	constructor(
		private _selfService: RoleService,
	) {
		super()
	}

	permisisonKeys: IPermissionConfig[] = [];
	permissionsTree: IPermissionTreeItem[] = [];

	selecteds: IPermissionConfig[] = [];
	cacheselecteds: IPermissionConfig[] = [];
	selectedOlds: string[] = [];

	parentNode: IPermissionConfig[] =[];
	roleId: number;

	ngOnInit(): void {
		//
		this.permisisonKeys = this.dialogConfig.data.permisisonKeys;
		this.permissionsTree = this.dialogConfig.data.permissionsTree;
		this.roleId = this.dialogConfig.data.id;
		//
		this.form = this.fb.group({
			id: [this.roleId],
			name: [null, required()],
			description: null,
			permissionKeys: [],
			permissionKeysRemove: [],
		});
		//
		this.isEdit = true;
		if(!this.roleId) {
            this.form.removeControl('id');
        } else {
			this.getRole();
		}
	}

	getRole() {
		this.isLoading = true;
		this._selfService.findById(this.roleId)
		.pipe(finalize(() => this.isLoading = false))
		.subscribe((res) => {
			if(this.checkStatusResponse(res)) {
				let rolePermisions: string[] = res?.data?.permissionKeys || [];
				this.cacheselecteds = this.selecteds = this.permisisonKeys.filter(p => rolePermisions.includes(p.key));
				this.selectedOlds = [...this.selecteds.map(s => s.key)];
				this.form.patchValue(res?.data);
			}
		})
	}

	
	// SHOW TẤT CẢ CÁC NODE CÓ TRONG CÂY (TREE)
	showNodes(node:TreeNode, isExpand:boolean) {
		node.expanded = isExpand;
		if (node.children) {
			node.children.forEach( childNode => {
				this.showNodes(childNode, isExpand);
			} );
		}
	}

	//  SELECT
	selectedNote(node) {
		this.customDataSelected(node);
	}

	// UNSELECT
	unSelectedNote(node) {
		this.customDataSelected(node, true);
	}

    // SINGLE SELECT
    singleSelect(node:any){
		if(node){
            this.cacheselecteds = this.selecteds = [...this.cacheselecteds, ...this.selecteds];
            this.parentNode = [];
            this.getParentNode(node);
            this.filterItemNodeDuplidate();
            
            //
            if(this.parentNode.length) {
                this.parentNode.forEach(node => {
                    let issetNode = this.selecteds.find(s => s.key == node.key);
                    if(!issetNode) this.selecteds.push(node);
                });
            }
        }
	}

	// CUSTOM BASE XỬ LÝ UNSELECT NODE CON KO UNSELECT NODE CHA
	customDataSelected(node, unSelected: boolean = false) {
		// Xử lý bước 1 (Dùng cho cả Select và unSelect)
		this.parentNode = [];
		this.getParentNode(node);
		this.filterItemNodeDuplidate();
		//
		if(this.parentNode.length) {
		this.parentNode.forEach(node => {
			let issetNode = this.selecteds.find(s => s.key == node.key);
			if(!issetNode) this.selecteds.push(node);
		});
		}
		// Xử lý bước 2 (Chỉ dùng cho unSelect)
		// Comment bỏ tích cha khi bỏ hết tích con
		// if(unSelected) this.unSelectParentNode(node?.parent);
		this.cacheselecteds = this.selecteds;
	}

	// UNSELECT NODE CHA KHI CÁC NODE CON BỊ UNSELECT HẾT
	unSelectParentNode(parentNode) {
		if(parentNode) {
			let unSelect = true;
			//
			parentNode.children.forEach(childNode => {
			  let childNodeIssetSelected = this.selecteds.find(s => s.key == childNode.key);
			  if(childNodeIssetSelected) unSelect = false;
			});
			//
			if(unSelect) {
			  let indexNode = this.selecteds.findIndex(s => s.key == parentNode.key);
			  if(indexNode >= 0) this.selecteds.splice(indexNode, 1);
			  if(parentNode.parent) this.unSelectParentNode(parentNode.parent);
			}
			//
		}
	}

	// LẤY DANH SÁCH TỔ TIÊN CHA, ÔNG, CỤ, KỴ... CỦA NODE
	getParentNode(node) {
		if(node?.parent) {
			this.parentNode.push(node.parent);
			this.getParentNode(node.parent);
		}  
	}
	
    // LỌC PHẦN TỬ DUPLICATE TRONG DANH SÁCH NODE SELECTED
    filterItemNodeDuplidate() {
		let keyIsset = []
		this.selecteds.forEach((item, index) => {
		  if(!keyIsset.includes(item.key)) {
			keyIsset.push(item.key);
		  } else {
			this.selecteds.splice(index, 1);
		  }
		});
	}

	save(event: IEventSubmitForm) {
		if (!this.inValidForm()) {
			const permissionKeys: string[] = this.selecteds.map(s => s.key);
			this.form.get('permissionKeys').setValue(permissionKeys);
			let permissionKeysRemove: string[] = [];
			//
			if(this.selectedOlds?.length) {
				permissionKeysRemove = this.selectedOlds.filter(keyOld => !this.formValue()['permissionKeys'].includes(keyOld));
			}
			this.form.get('permissionKeysRemove').setValue(permissionKeysRemove);
			//
			let body: any = this.formValue();
			// Thêm mới
			if(!this.roleId) {
				this._selfService.create(body).subscribe((res) => {
					if(this.checkStatusResponse(res, 'Thêm thành công!')) {
						this.closeDialog(true);
					}
				})
			} // Cập nhật
			else {
				this._selfService.update(body).subscribe((res) => {
					if(this.checkStatusResponse(res, 'Cập nhật thành công!')) {
						this.closeDialog(true);
					}
				})
			}
		}
	}
}
