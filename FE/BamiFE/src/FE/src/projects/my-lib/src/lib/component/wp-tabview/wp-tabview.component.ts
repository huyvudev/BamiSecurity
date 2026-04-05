import { HttpParams } from '@angular/common/http';
import { AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, Component, ContentChildren, EventEmitter, Input, OnChanges, OnInit, Output, QueryList, SimpleChanges, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TabPanel, TabView } from 'primeng/tabview';
import { LibHelperService } from '../../shared/services/lib-helper.service';

@Component({
    selector: 'lib-tabView',
    styleUrls: ['./wp-tabview.component.scss'],
	template: `
		<ng-container>
			<div [id]="id" class="{{ isInit ? 'disable-active-tab' : '' }}">
				<ng-content></ng-content>
			</div>
		</ng-container>
	`,
    changeDetection: ChangeDetectionStrategy.OnPush
})
//
export class WpTabviewComponent implements OnInit, OnChanges, AfterViewInit {

    constructor(
        private route: ActivatedRoute,
        private router: Router,
		private _libHelperService: LibHelperService,
		private cd: ChangeDetectorRef,
    ) {
        this.selectedTabName = this.route.snapshot.queryParamMap.get('selectedTabName');
        console.log('selectedTabName', this.selectedTabName);
    }

    selectedTabName: string;

	@Input() isEditTab: boolean = false;
	@Output() isEditTabChange = new EventEmitter<boolean>(false);

	@Input() confirmChangeTab: boolean = true;
	@Input() messageConfirmChangeTab: string = 'Bạn chắc chắn muốn chuyển Tab?';

    @Output() setActiveIndex = new EventEmitter<any>(null);
    //
    @Input() tabViewActive: { [key: string]: boolean };
    @Output() setSelectedTab = new EventEmitter<string>(null);

    @Input() onChange: number;
    @Input() tabView: TabView;

    id: string;

    ngOnInit(): void {
        this.id = 'tabview-' + new Date().getTime();
    }

    ngOnChanges(changes: SimpleChanges): void {
    }

    listTabViewActive: string[] = [];
    isInit: boolean = true;
	isDisableStyleTabChange: boolean = false;
    
    ngAfterViewInit(): void {
        setTimeout(() => {
            const wpTabViewEl: any = document.getElementById(this.id);
			console.log('wpTabViewEl', wpTabViewEl);
			
            const tabPanelEls: any = wpTabViewEl.getElementsByTagName('p-tabpanel') as HTMLElement;
            this.listTabViewActive = [...tabPanelEls]?.map(el => el?.id);
            //
            this.initActiveTab();
        }, 0);
    }

    resetTab() {
        for(let tabName in this.tabViewActive) {
            this.tabViewActive[tabName] = false;
        }
    }

	handleOpenTab(event: Event, tab: TabPanel) {
		const tabIndexChange = this.tabView.findTabIndex(tab);
		const currentTabName = this.listTabViewActive[this.getIndexTab()];
		const tabNameChange = this.listTabViewActive[tabIndexChange];
		//
		if(this.confirmChangeTab && this.isEditTab) {
			this._libHelperService.dialogConfirmRef(this.messageConfirmChangeTab)
			.onClose.subscribe((res) => {
				if(res?.accept) {
					TabView.prototype.open.apply(this.tabView, arguments);
					this.isEditTabChange.emit(false);
					this.tabViewActive[currentTabName] = false;
					this.activeTab(tabNameChange);
				}
			})
		} else {
			TabView.prototype.open.apply(this.tabView, arguments);
			this.activeTab(tabNameChange);
		}
	}

    initActiveTab() {
        const activeIndex = this.getIndexTab();
        this.tabView.activeIndexChange.emit(activeIndex);
		//
		this.selectedTabName = this.listTabViewActive[activeIndex];
        this.tabViewActive[this.selectedTabName] = true;
		this.tabView.open = this.handleOpenTab.bind(this);
		//
		setTimeout(() => {
			this.isInit = false;
			this.cd.detectChanges();
		}, 0);
		//
        this.activeTab()
    }

    getIndexTab() {
        const index = this.listTabViewActive.findIndex((name) => name === (this.selectedTabName));
        return index !== -1 ? index : 0;
    }

	activeTab(tabNameChange?: string) {
		this.selectedTabName = tabNameChange || this.selectedTabName;
		this.setSelectedTab.emit(this.selectedTabName);
		setTimeout(() => {
			// Nhét vào timeout để tránh xung đột vs changeDetection khi thay đổi cảnh báo warning type data change
			if(!this.tabViewActive[this.selectedTabName]) {
				this.tabViewActive[this.selectedTabName] = true;
			}
			let params = new HttpParams().append('selectedTabName', this.selectedTabName);
			const urlChange = window.location.pathname + `?${params.toString()}`
			this.router.navigateByUrl(urlChange);
		}, 0);
	}
}
