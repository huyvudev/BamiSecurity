import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ETypeHandleLinkYoutube, ETypeUrlYoutube } from '../shared/consts/base.consts';
import { Utils } from '../shared/utils';

@Pipe({
    name: 'handleLinkYoutube'
})
export class HandleLinkYoutubePipe implements PipeTransform {

    constructor(
        public sanitizer: DomSanitizer
    ) {}

    urlYoutubes = ETypeUrlYoutube;

    transform(url: string, type: ETypeHandleLinkYoutube) {
        if(type === ETypeHandleLinkYoutube.CHECK_LINK) return this.checkLinkYoutube(url);
        if(type === ETypeHandleLinkYoutube.GET_ID) return this.getIdVideoYoutube(url);
        if(type === ETypeHandleLinkYoutube.GET_EMBED_LINK) return this.getYoutubeEmbed(url);
        return null;
    }

    checkLinkYoutube(link: string): boolean {
        let isCheck: boolean = false;
        const urlYoutubes = {
            ...this.urlYoutubes,
            ORIGIN: "https://www.youtube.com"
        }
        //
        try {
            if(typeof link === 'string') {
                for(const [key, url] of Object.entries(urlYoutubes)) {
                    isCheck = link?.includes(url);
                    if(isCheck) break;
                }
            }
            //
            return isCheck;
        } catch (error) {
            Utils.log('checkLinkYoutube', error);
            return false;
        }
    }

    getIdVideoYoutube(link: string): string {
        try {
            let videoId: string;
            for(const key in this.urlYoutubes) {
                if(key === 'WATCH' && link.includes('?v=')) {
                    videoId = link.split('v=')[1];
                    const ampersandPosition = videoId.indexOf('&');
                    if(ampersandPosition != -1) {
                        videoId = videoId.substring(0, ampersandPosition);
                    }
                } else {
                    videoId = link.split('/').pop();
                    const questionMark = videoId.indexOf('?');
                    if(questionMark != -1) {
                        videoId = videoId.substring(0, questionMark);
                    }
                }
                //
                if(videoId) break;
            }
            return videoId;
        } catch (error) {
            Utils.log('getIdVideoYoutube', error);
            return "";
        }
    }

    getLinkWatchYoutube(link:string) {
        let videoId = this.getIdVideoYoutube(link);
        // if(videoId) return ETypeUrlYoutube.WATCH+'?v='+videoId;
        if(videoId) return 'https://www.youtube.com/embed/'+videoId;
        return "";
    }

    getYoutubeEmbed(link: string) {
        // const url = 'https://www.youtube.com/embed/'+this.getIdVideoYoutube(link);
        const url = 'https://www.youtube-nocookie.com/embed/'+this.getIdVideoYoutube(link);
        return this.sanitizer.bypassSecurityTrustResourceUrl(url);
    }

}
