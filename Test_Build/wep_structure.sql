/*
 Navicat MySQL Data Transfer

 Source Server         : WoW_MaNGOS
 Source Server Type    : MySQL
 Source Server Version : 50558
 Source Host           : localhost:3306
 Source Schema         : wep

 Target Server Type    : MySQL
 Target Server Version : 50558
 File Encoding         : 65001

 Date: 17/01/2018 14:44:42
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for accounts
-- ----------------------------
DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts`  (
  `id` int(32) NOT NULL AUTO_INCREMENT,
  `username` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `password` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `nickname` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `email` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `accesslevel` int(255) NULL DEFAULT 1,
  `online` int(255) NULL DEFAULT 0,
  `exp` int(255) NULL DEFAULT 0,
  `dinar` int(255) NULL DEFAULT 75000,
  `kills` int(255) NULL DEFAULT 0,
  `deaths` int(255) NULL DEFAULT 0,
  `premium` int(255) NULL DEFAULT 0,
  `premiumExpire` int(255) NULL DEFAULT -1,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Table structure for equipment
-- ----------------------------
DROP TABLE IF EXISTS `equipment`;
CREATE TABLE `equipment`  (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `owner` int(10) UNSIGNED NOT NULL,
  `class1` varchar(40) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'DA02,DB01,DF01,DR01,^,^,^,^',
  `class2` varchar(40) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'DA02,DB01,DF01,DQ01,^,^,^,^',
  `class3` varchar(40) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'DA02,DB01,DG05,DN01,^,^,^,^',
  `class4` varchar(40) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'DA02,DB01,DC02,DN01,^,^,^,^',
  `class5` varchar(40) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'DA02,DB01,DJ01,DL01,^,^,^,^',
  UNIQUE INDEX `id`(`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Table structure for inventory
-- ----------------------------
DROP TABLE IF EXISTS `inventory`;
CREATE TABLE `inventory`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `owner` int(255) NULL DEFAULT NULL,
  `code` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `expiredate` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Table structure for serverlist
-- ----------------------------
DROP TABLE IF EXISTS `serverlist`;
CREATE TABLE `serverlist`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `ip` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `flag` int(255) NULL DEFAULT 1,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Compact;

SET FOREIGN_KEY_CHECKS = 1;
