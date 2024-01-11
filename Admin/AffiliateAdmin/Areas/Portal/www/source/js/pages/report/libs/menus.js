import React from 'react'
import { menuKey, relativePath } from './constants'
import { permissionResource } from '../../../modules/permissions'

const AffiliateReport = React.lazy(() => import(/* webpackChunkNamw: 'affiliateReport'*/ '../affiliate-report/index'))
const PlayerGgrReport = React.lazy(() => import(/* webpackChunkNamw: 'playerGgrReport'*/ '../player-ggr-report/index'))
const TransferReport = React.lazy(() => import(/* webpackChunkNamw: 'transferReport'*/ '../transfer-report/index'))

export const menus = [
    { key: menuKey.affiliateReport, label: 'Affiliate', href: relativePath.affiliateReport, component: AffiliateReport, permissionName: permissionResource.AffReport },
    { key: menuKey.playerGgrReport, label: 'Player GGR', href: relativePath.playerGgrReport, component: PlayerGgrReport, permissionName: permissionResource.AffReport },
    { key: menuKey.transferReport, label: 'Transfer Balance For Player', href: relativePath.transferReport, component: TransferReport, permissionName: permissionResource.TransferPlayerReport },
]