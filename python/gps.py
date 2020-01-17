# -*- coding: utf-8 -*- 
import sys
import os
import time
import utm
import math
import matplotlib.pyplot as plt
import heapq
import pandas as pd

COLOR_LIST = ["k", "r", "b", "g", "m", "c", "y"]

class GpsDataAnalysis(object):
    def __init__(self, directory_path):
        self.directory_path = directory_path
        self.gnrmc_list = list()
        self.latLon_list = list()
        self.utm_list = list()
        self.time_list = list()
        self.vel_list = list()
        self.heading_list = list()
        self.position_list = list()
        self.point_list_x = list()
        self.point_list_y = list()
        self.point_list_x_2 = list()
        self.point_list_y_2 = list()

        
    def process_data(self):
        print("====process_data()====")
        self.read_data()
        self.transform2LonLat()
        self.transform2Utm()
        self.position_process()
        self.compute_distance()
        #self.position_process()
        self.plot_data()

    def compute_distance(self):
        line_1 = self.utm_list[0]
        line_2 = self.utm_list[1]
        length_1 = len(line_1["x"])
        length_2 = len(line_2["x"])
        distance_point2line_list = list()
        distance_point2line_out_list = list()
        index_list = list()
        for i in range(length_1):
            point1 = (line_1["x"][i], line_1["y"][i])
            distance_list = list()
            for j in range(length_2):
                point2 = (line_2["x"][j], line_2["y"][j])
                distance_list.append(self.compute_distanceWithoutLine(point1, point2))
            l = list()
            min_index = distance_list.index(min(distance_list))
            l.append(min_index)
            distance_list[min_index] = 100
            min_index = distance_list.index(min(distance_list))
            l.append(min_index)
            l.sort()
            p1 = (line_2["x"][l[0]], line_2["y"][l[0]])
            p2 = (line_2["x"][l[1]], line_2["y"][l[1]])
            
            A, B, C = self.compute_line(p1, p2)
            distance = self.compute_distanceWithLine(A, B, C, point1)
            distance_point2line_list.append(distance)
            if distance > 3.75:
                self.point_list_x_2.append(line_2["x"][l[0]])
                self.point_list_y_2.append(line_2["y"][l[0]])
                self.point_list_x_2.append(line_2["x"][l[1]])
                self.point_list_y_2.append(line_2["y"][l[1]])
                distance_point2line_out_list.append(distance)
                self.point_list_x.append(line_1["x"][i])
                self.point_list_y.append(line_1["y"][i])
                index_list.append(i) 
                print(point1[0] - line_2["x"][l[0]])
                print(point1[1] - line_2["y"][l[0]])
                print(point1[0] - line_2["x"][l[1]])
                print(point1[1] - line_2["y"][l[1]])
        print(length_1, len(distance_point2line_out_list), len(self.point_list_x_2))
        print(distance_point2line_out_list)
        df = pd.DataFrame(distance_point2line_out_list)
        print(df.describe())


    def position_process(self):
        for gnrmc_dict in self.gnrmc_list:
            time_list = list()
            vel_list = list()
            heading_list = list()
            for key, value in gnrmc_dict.items():
                if key == "utcT":
                    for i in range(len(value)-1):
                        time_list.append((\
                            float(value[i+1][:2])-float(value[i][:2]))*3600 + \
                                (float(value[i+1][2:4])-float(value[i][2:4]))*60 + \
                                    (float(value[i+1][4:])-float(value[i][4:])))
                if key == "vel":
                    for vel in value:
                        vel_list.append(float(vel) * 1.852 / 3.6)
                if key == "angle":
                    for angle in value:
                        heading_list.append(angle)
            self.vel_list.append(vel_list)
            self.time_list.append(time_list)
            self.heading_list.append(heading_list)
        for i in range(len(self.utm_list)):
            utm_dict = self.utm_list[i]
            time_list = self.time_list[i]
            vel_list = self.vel_list[i]
            heading_list = self.heading_list[i]
            position_dict = dict()
            position_dict["x"] = list()
            position_dict["y"] = list()
            for key, value in utm_dict.items():
                if key == "x":
                    X = value[0]
                    position_dict["x"].append(X)
                    for i in range(len(value)-1):
                        if heading_list[i]:
                            X = X + math.sin(float(heading_list[i])/180 * math.pi) * vel_list[i] * time_list[i]
                            position_dict["x"].append(X)
                        else:
                            position_dict["x"].append(value[i])
                            X = value[i]
                    position_dict["x"].append(value[-1])
                if key == "y":
                    Y = value[0]
                    position_dict["y"].append(Y)
                    for i in range(len(value)-1):
                        if heading_list[i]:
                            Y = Y + math.cos(float(heading_list[i])/180 * math.pi) * vel_list[i] * time_list[i]
                            position_dict["y"].append(Y)
                        else:
                            position_dict["y"].append(value[i])
                            Y = value[i]
                    position_dict["y"].append(value[-1])
            self.position_list.append(position_dict)
    def read_data(self):
        print("====read_data()====")
        for root, dirs, files in os.walk(self.directory_path):
            files.sort()
            for file in files[:]:
                file_path = root + file
                with open(file_path, "r", encoding='utf-8') as read_file:
                    self.set_gnrmc_list(read_file)
                    read_file.close()
                
            break
    def transform2LonLat(self):
        print("====transform2LonLat()====")
        for gnrmc_dict in self.gnrmc_list:
            latLon_dict = dict()
            latLon_dict["lat"] = list()
            latLon_dict["lon"] = list()
            for key, value in gnrmc_dict.items():
                if (key == "lat"):
                    for lat in value:
                        if (lat):
                            latLon_dict["lat"].append(float(lat.split(".")[0][:2]) + float("%.6f" % (float(lat[2:]) / 60)))
                if (key == "lon"):
                    for lon in value:
                        if (lon):
                            latLon_dict["lon"].append(float(lon.split(".")[0][:3]) + float("%.6f" % (float(lon[3:]) / 60)))   
            self.latLon_list.append(latLon_dict)
    def transform2Utm(self):
        print("====transform2Utm()====")
        for latLon_dict in self.latLon_list:
            utm_dict = dict()
            utm_dict["x"] = list()
            utm_dict["y"] = list()
            for key, value in latLon_dict.items():
                if (key == "lat"):
                    lat_list = value
                if (key == "lon"):
                    lon_list = value
            for i in range(len(lat_list)):
                utm_data = utm.from_latlon(lat_list[i], lon_list[i])
                utm_dict["x"].append(utm_data[0])
                utm_dict["y"].append(utm_data[1])
            self.utm_list.append(utm_dict)
    def plot_data(self):
        print("====plot_data()====")
        plt.figure()
        plt.title("2020-01-09")
        plt.xlabel("x")
        plt.ylabel("y")
        for i in range(len(self.utm_list)):
            utm_dict = self.utm_list[i]
            for key, value in utm_dict.items():
                if (key == "x"):
                    x_list = value
                if (key == "y"):
                    y_list = value
            plt.plot(x_list, y_list, COLOR_LIST[i])
        for i in range(len(self.position_list)):
            position_dict = self.position_list[i]
            for key, value in position_dict.items():
                if (key == "x"):
                    xx_list = value
                if (key == "y"):
                    yy_list = value
            #plt.plot(xx_list, yy_list, COLOR_LIST[i]+"o--")
        plt.plot(self.point_list_x, self.point_list_y, "ro")
        plt.plot(self.point_list_x_2, self.point_list_y_2, "ko")
        #labels = ["origin_data(-)", "compute_data(-o-)"]
        #plt.legend(loc=2, labels=labels)
        plt.show()
    def set_gnrmc_list(self, read_file):
        print("====set_gnrmc_list()====")
        gnrmc_dict = dict()
        gnrmc_dict["utcT"] = list() # UTC time  
        gnrmc_dict["posF"] = list() # position fixed: A is true, V is false
        gnrmc_dict["lat"] = list()  # latitude
        gnrmc_dict["tD"] = list()   # latitude direction E/W
        gnrmc_dict["lon"] = list() # longitude
        gnrmc_dict["nD"] = list()   # longitude direction N/S
        gnrmc_dict["vel"] = list()  # velocity 1.852km／h
        gnrmc_dict["angle"] = list()    # angle
        gnrmc_dict["utcD"] = list() # utc date
        gnrmc_dict["magV"] = list() # Mag Var  (000-180)
        gnrmc_dict["mD"] = list()   # Mag Var direction
        gnrmc_dict["mode"] = list() # 
        for index, value in enumerate(read_file.readlines()):
            gnrmc_data = value.split("$")[1].split(",")
            if gnrmc_data[2] == "A":
                gnrmc_dict["utcT"].append(gnrmc_data[1])
                gnrmc_dict["posF"].append(gnrmc_data[2])
                gnrmc_dict["lat"].append(gnrmc_data[3])
                gnrmc_dict["tD"].append(gnrmc_data[4])
                gnrmc_dict["lon"].append(gnrmc_data[5])
                gnrmc_dict["nD"].append(gnrmc_data[6])
                gnrmc_dict["vel"].append(gnrmc_data[7])
                gnrmc_dict["angle"].append(gnrmc_data[8])
                gnrmc_dict["utcD"].append(gnrmc_data[9])
                gnrmc_dict["magV"].append(gnrmc_data[10])
                gnrmc_dict["mD"].append(gnrmc_data[11])
                gnrmc_dict["mode"].append(gnrmc_data[12])
        self.gnrmc_list.append(gnrmc_dict)


    def compute_distanceWithLine(self, A, B, C, point):
        return abs(A * point[0] + B * point[1] + C) / math.sqrt((pow(A, 2) + pow(B, 2)))

    def compute_distanceWithoutLine(self, p1, p2):
        return math.sqrt(pow(p2[0] - p1[0], 2) + pow(p2[1] - p1[1], 2))

    def compute_line(self, p1, p2):
        if p1[0] == p2[0]:
            return (1, 0, -p1[0])
        if p1[1] == p2[1]:
            return (0, 1, -p1[1])
        k = (p2[1] - p1[1]) / (p2[0] - p1[0])
        A = k
        B = -1.0
        C = -k * p1[0] + p1[1]
        return A, B, C
    
def main():
    directory_path = "E:\\data\\2020-01-16\\"
    gpsDataAnalysis = GpsDataAnalysis(directory_path)
    gpsDataAnalysis.process_data()
    
    

if __name__ == "__main__":
    sys.exit(main())