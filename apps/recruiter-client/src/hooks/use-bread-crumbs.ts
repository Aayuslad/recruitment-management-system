import { routes } from '@/config/route-config';
import { matchRoutes, useLocation, useParams } from 'react-router-dom';

export function useBreadCrumbs() {
    const location = useLocation();
    const params = useParams();

    const matches = matchRoutes(routes, location);

    if (!matches) return [];

    return matches.map((match) => {
        const { route } = match;
        const label =
            typeof route.breadcrumb === 'function'
            //@ts-ignore
                ? route.breadcrumb(params)
                : route.breadcrumb;

        return { label, path: match.pathname };
    });
}
