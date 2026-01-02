import { Tabs, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { useAppStore } from '@/store';
import { JobApplicationIndexTabs as tabs } from '@/store/job-application-store';
import { useShallow } from 'zustand/react/shallow';

export const JobApplicationIndexTabBar = () => {
    const {
        currentJobApplicationTab: currentTab,
        setCurrentJobApplicationTab: setCurrentTab,
    } = useAppStore(
        useShallow((s) => ({
            currentJobApplicationTab: s.currentJobApplicationTab,
            setCurrentJobApplicationTab: s.setCurrentJobApplicationTab,
        }))
    );

    return (
        <div className="">
            <Tabs defaultValue={currentTab} className="gap-4">
                <TabsList className="bg-background rounded-none border-b p-0">
                    {tabs.map((tab) => (
                        <TabsTrigger
                            key={tab.value}
                            value={tab.value}
                            onClick={() => {
                                setCurrentTab(tab.value);
                            }}
                            className="bg-background data-[state=active]:border-primary dark:data-[state=active]:border-primary h-full px-5 rounded-none border-0 border-b-2 border-transparent data-[state=active]:shadow-none"
                        >
                            {tab.label}
                        </TabsTrigger>
                    ))}
                </TabsList>
            </Tabs>
        </div>
    );
};
